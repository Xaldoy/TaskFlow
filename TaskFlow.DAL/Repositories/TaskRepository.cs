﻿using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Models;

namespace DAL.Repositories
{
    public class TaskRepository(TaskFlowContext context) : ITaskRepository
    {
        private readonly TaskFlowContext _context = context;


        public async Task<List<TaskCategory>> GetTaskCategories(string userId)
        {
            return await _context.TaskCategories
                .Where(x => x.OwnerId == userId)
                .ToListAsync();
        }

        public async Task<List<TaskItem>> GetTasksByCategory(int categoryId)
        {
            return await _context.TaskItems
                .Include(t => t.TaskPriority)
                .Where(x => x.TaskCategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<TaskCategory?> GetCategoryById(int categoryId)
        {
            return await _context.TaskCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == categoryId);
        }

        public async Task<TaskItem> CreateTask(TaskItem taskItem)
        {
            var createdTaskItem = await _context.TaskItems.AddAsync(taskItem);
            await _context.SaveChangesAsync();
            return createdTaskItem.Entity;
        }

        public async Task DeleteTask(int taskItemId)
        {
            var taskItem = await _context.TaskItems.FindAsync(taskItemId);
            if (taskItem == null) return;
            _context.Remove(taskItem);
            await _context.SaveChangesAsync();
        }

        public async Task<TaskItem> UpdateTask(TaskItem taskItem)
        {
            var updatedTaskItem = _context.Update(taskItem);
            await _context.SaveChangesAsync();
            return updatedTaskItem.Entity;
        }

        public async Task<TaskItem?> GetTaskById(int taskId)
        {
            return await _context.TaskItems
                .Include(x => x.TaskCategory)
                .Include(x => x.TaskPriority)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == taskId);
        }

        public async Task<TaskCategory> CreateTaskCategory(TaskCategory taskCategory)
        {
            var createdTaskCategory = await _context.TaskCategories.AddAsync(taskCategory);
            await _context.SaveChangesAsync();
            return createdTaskCategory.Entity;
        }

        public async Task DeleteTaskCategory(int categoryId)
        {
            var category = await _context.TaskCategories.FindAsync(categoryId);
            if (category == null) return;
            _context.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TaskPriority>> GetTaskPriorities()
        {
            var priorites = await _context.TaskPriorities.ToListAsync();
            return priorites;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using TaskFlow.Service.Services.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TaskController(ITaskService taskService) : BaseController
    {
        private readonly ITaskService _taskService = taskService;

        [HttpGet]
        public async Task<IActionResult> GetTaskCategories()
        {
            var taskCategories = await _taskService.GetTaskCategories();
            return HandleServiceResult(taskCategories);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasksForCategory(int categoryId)
        {
            var tasksForCategory = await _taskService.GetTasksForCategory(categoryId);
            return HandleServiceResult(tasksForCategory);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskItemDto taskItem)
        {
            var createdTask = await _taskService.CreateTask(taskItem);
            return HandleServiceResult(createdTask);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTask(TaskItemDto taskItem)
        {
            var updatedTask = await _taskService.UpdateTask(taskItem);
            return HandleServiceResult(updatedTask);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTask(int taskItemId)
        {
            var result = await _taskService.DeleteTask(taskItemId);
            return HandleServiceResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskCategory(TaskCategoryDto taskCategory)
        {
            var createdTaskCategory = await _taskService.CreateTaskCategory(taskCategory);
            return HandleServiceResult(createdTaskCategory);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTaskCategory(int categoryId)
        {
            var result = await _taskService.DeleteTaskCategory(categoryId);
            return HandleServiceResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskPriorities()
        {
            var taskPriorities = await _taskService.GetTaskPriorities();
            return HandleServiceResult(taskPriorities);
        }
    }
}

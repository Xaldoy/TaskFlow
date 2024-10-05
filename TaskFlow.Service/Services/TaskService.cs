using AutoMapper;
using DAL.Interfaces;
using Model.Models;
using Service.Authorization;
using Service.DTOs;
using Service.DTOs.Result;
using Service.Interfaces;
using Service.Utility;

namespace Service.Services
{
    public class TaskService(IMapper mapper, ITaskRepository taskRepository, IAuthorizationService authorizationService) : ITaskService
    {
        private readonly IMapper _mapper = mapper;
        private readonly ITaskRepository _taskRepository = taskRepository;
        private readonly IAuthorizationService _authorizationService = authorizationService;

        public async Task<ServiceResult<List<TaskCategoryDto>>> GetTaskCategories()
        {
            ServiceResult<string> userIdServiceResult = _authorizationService.GetUserId();
            if (userIdServiceResult.IsError || userIdServiceResult.Data == null)
                return ServiceResult<List<TaskCategoryDto>>.Failure(userIdServiceResult.Error);

            var taskCategories = _mapper.Map<List<TaskCategoryDto>>(await _taskRepository.GetTaskCategories(userIdServiceResult.Data));
            return ServiceResult<List<TaskCategoryDto>>.Success(taskCategories);
        }

        public async Task<ServiceResult<List<TaskItemDto>>> GetTasksForCategory(int categoryId)
        {
            if (!await _authorizationService.UserOwnsTaskCategory(categoryId))
                return ServiceResult<List<TaskItemDto>>.Failure(ErrorDescriber.Unauthorized());

            var taskItems = _mapper.Map<List<TaskItemDto>>(await _taskRepository.GetTasksByCategory(categoryId));
            return ServiceResult<List<TaskItemDto>>.Success(taskItems);
        }

        public async Task<ServiceResult<TaskItemDto>> CreateTask(TaskItemDto taskItem)
        {
            if (!await _authorizationService.UserOwnsTaskCategory(taskItem.TaskCategoryId))
                return ServiceResult<TaskItemDto>.Failure(ErrorDescriber.Unauthorized());

            var createdTask = _mapper.Map<TaskItemDto>(await _taskRepository.CreateTask(_mapper.Map<TaskItem>(taskItem)));
            return ServiceResult<TaskItemDto>.Success(createdTask);
        }

        public async Task<ServiceResult> DeleteTask(int taskItemId)
        {
            if (!await _authorizationService.UserOwnsTask(taskItemId))
                return ServiceResult.Failure(ErrorDescriber.Unauthorized());

            await _taskRepository.DeleteTask(taskItemId);
            return ServiceResult.Success();
        }

        public async Task<ServiceResult<TaskItemDto>> UpdateTask(TaskItemDto taskItem)
        {
            if (!await _authorizationService.UserOwnsTask(taskItem.Id))
                return ServiceResult<TaskItemDto>.Failure(ErrorDescriber.Unauthorized());

            var updatedTask = _mapper.Map<TaskItemDto>(await _taskRepository.UpdateTask(_mapper.Map<TaskItem>(taskItem)));
            return ServiceResult<TaskItemDto>.Success(updatedTask);
        }

        public async Task<ServiceResult<TaskCategoryDto>> CreateTaskCategory(TaskCategoryDto taskCategory)
        {
            var userIdServiceResult = _authorizationService.GetUserId();
            if (userIdServiceResult.IsError || userIdServiceResult.Data == null)
                return ServiceResult<TaskCategoryDto>.Failure(userIdServiceResult.Error);

            var taskToCreate = _mapper.Map<TaskCategory>(taskCategory);
            taskToCreate.OwnerId = userIdServiceResult.Data;

            var createdTask = _mapper.Map<TaskCategoryDto>(await _taskRepository.CreateTaskCategory(taskToCreate));
            return ServiceResult<TaskCategoryDto>.Success(createdTask);
        }

        public async Task<ServiceResult> DeleteTaskCategory(int categoryId)
        {
            if (!await _authorizationService.UserOwnsTaskCategory(categoryId))
                return ServiceResult.Failure(ErrorDescriber.Unauthorized());

            await _taskRepository.DeleteTaskCategory(categoryId);
            return ServiceResult.Success();
        }

        public async Task<ServiceResult<List<TaskPriorityDto>>> GetTaskPriorities()
        {
            var taskPriorities = _mapper.Map<List<TaskPriorityDto>>(await _taskRepository.GetTaskPriorities());
            return ServiceResult<List<TaskPriorityDto>>.Success(taskPriorities);
        }
    }
}

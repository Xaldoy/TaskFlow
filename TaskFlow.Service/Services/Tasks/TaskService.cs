using AutoMapper;
using Model.Models;
using Service.DTOs.Result;
using TaskFlow.DAL.Repositories.Tasks;
using TaskFlow.Service.DTOs.Message;
using TaskFlow.Service.DTOs.Task;
using TaskFlow.Service.Services.Authorization;

namespace TaskFlow.Service.Services.Tasks
{
    public class TaskService(IMapper mapper, ITaskRepository taskRepository, IAuthorizationService authorizationService) : ITaskService
    {
        private readonly IMapper _mapper = mapper;
        private readonly ITaskRepository _taskRepository = taskRepository;
        private readonly IAuthorizationService _authorizationService = authorizationService;

        public async Task<ServiceResult> GetTaskCategories()
        {
            string userId = _authorizationService.GetUserId();
            var taskCategories = _mapper.Map<List<TaskCategoryDto>>(await _taskRepository.GetTaskCategories(userId));
            return ServiceResult.Success(taskCategories);
        }

        public async Task<ServiceResult> GetTasksForCategory(int categoryId)
        {
            if (!await _authorizationService.UserOwnsTaskCategory(categoryId))
                return ServiceResult.Failure(MessageDescriber.Unauthorized());

            var taskItems = _mapper.Map<List<TaskItemDto>>(await _taskRepository.GetTasksByCategory(categoryId));
            return ServiceResult.Success(taskItems);
        }

        public async Task<ServiceResult> CreateTask(TaskItemDto taskItem)
        {
            if (!await _authorizationService.UserOwnsTaskCategory(taskItem.TaskCategoryId))
                return ServiceResult.Failure(MessageDescriber.Unauthorized());

            var createdTask = _mapper.Map<TaskItemDto>(await _taskRepository.CreateTask(_mapper.Map<TaskItem>(taskItem)));
            return ServiceResult.Success(createdTask);
        }

        public async Task<ServiceResult> DeleteTask(int taskItemId)
        {
            if (!await _authorizationService.UserOwnsTask(taskItemId))
                return ServiceResult.Failure(MessageDescriber.Unauthorized());

            await _taskRepository.DeleteTask(taskItemId);
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> UpdateTask(TaskItemDto taskItem)
        {
            if (!await _authorizationService.UserOwnsTask(taskItem.Id))
                return ServiceResult.Failure(MessageDescriber.Unauthorized());

            var updatedTask = _mapper.Map<TaskItemDto>(await _taskRepository.UpdateTask(_mapper.Map<TaskItem>(taskItem)));
            return ServiceResult.Success(updatedTask);
        }

        public async Task<ServiceResult> CreateTaskCategory(TaskCategoryDto taskCategory)
        {
            string userId = _authorizationService.GetUserId();

            var taskToCreate = _mapper.Map<TaskCategory>(taskCategory);
            taskToCreate.OwnerId = userId;

            var createdTask = _mapper.Map<TaskCategoryDto>(await _taskRepository.CreateTaskCategory(taskToCreate));
            return ServiceResult.Success(createdTask);
        }

        public async Task<ServiceResult> DeleteTaskCategory(int categoryId)
        {
            if (!await _authorizationService.UserOwnsTaskCategory(categoryId))
                return ServiceResult.Failure(MessageDescriber.Unauthorized());

            await _taskRepository.DeleteTaskCategory(categoryId);
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> GetTaskPriorities()
        {
            var taskPriorities = _mapper.Map<List<TaskPriorityDto>>(await _taskRepository.GetTaskPriorities());
            return ServiceResult.Success(taskPriorities);
        }
    }
}

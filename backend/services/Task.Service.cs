using backend.enums;
using backend.models;
using backend.models.DTOs;
using backend.repositories.interfaces;
using backend.services.exceptions;
using backend.services.interfaces;

namespace backend.services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository) => _taskRepository = taskRepository;

        public async Task<Guid> AddAsync(TaskInputDto taskDto)
        {
            var newTask = await _taskRepository.AddAsync(ConvertTaskInputDtoToModel(taskDto));

            return newTask;
        }

        public async Task<List<TaskDto>?> GetAllAsync()
        {
            var listTasks = await _taskRepository.GetAllAsync();

            if (listTasks is null) return null;

            return ConvertTaskModelToDtoList(listTasks);
        }

        public async Task<TaskDto?> GetByIdAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);

            if (task is null) return null;

            return ConvertTaskModelToDto(task);
        }

        public async Task<bool> UpdateAsync(Guid id, TaskInputDto taskUptDto)
        {
            var task = await _taskRepository.GetByIdAsync(id);

            if (task is null)
                throw new ServicesExceptions("Tarefa não encontrada");

            AssignedTaskDtoToModel(task, taskUptDto);

            return await _taskRepository.UpdateAsync(task);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);

            if (task is null)
                throw new ServicesExceptions("Tarefa não encontrada");

            return await _taskRepository.DeleteAsync(task);
        }

        private void AssignedTaskDtoToModel(TaskModel task, TaskInputDto taskDto)
        {
            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.Priority = (TaskPriority)taskDto.Priority;
            task.Status = (enums.TaskStatus)taskDto.Status;
        }

        private List<TaskDto> ConvertTaskModelToDtoList(List<TaskModel> model)
        {
            return model.Select(ConvertTaskModelToDto).ToList();
        }

        private TaskDto ConvertTaskModelToDto(TaskModel model)
        {
            return new TaskDto(
                Id: model.Id,
                UserId: model.UserId,
                Title: model.Title,
                Description: model.Description,
                Priority: model.Priority,
                Status: model.Status,
                CreatedAt: model.CreatedAt,
                UpdatedAt: model.UpdatedAt,
                CompletedAt: model.CompletedAt,
                CancelledAt: model.CancelledAt,
                UserEmail: model.User.Email
            );
        }

        private TaskModel ConvertTaskInputDtoToModel(TaskInputDto inputDto)
        {
            return new TaskModel()
            {
                Title = inputDto.Title,
                Description = inputDto.Description,
                Priority = (TaskPriority)inputDto.Priority,
                Status = (enums.TaskStatus)inputDto.Status
            };
        }

        private TaskInputDto ConvertTaskModelToInputDto(TaskModel task, TaskInputDto taskDto)
        {
            return new TaskInputDto(
                Title: task.Title,
                Description: task.Description,
                Priority: (int)task.Priority,
                Status:(int)task.Status
            );
        }
    }
}

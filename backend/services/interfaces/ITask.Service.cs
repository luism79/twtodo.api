using backend.models.DTOs;

namespace backend.services.interfaces
{
    public interface ITaskService
    {
        Task<Guid> AddAsync(TaskInputDto taskDto);

        Task<List<TaskDto>?> GetAllAsync();

        Task<TaskDto?> GetByIdAsync(Guid id);

        Task<bool> UpdateAsync(Guid id, TaskInputDto taskUptDto);

        Task<bool> DeleteAsync(Guid id);
    }
}

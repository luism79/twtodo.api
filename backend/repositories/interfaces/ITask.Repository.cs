using backend.models;

namespace backend.repositories.interfaces
{
    public interface ITaskRepository
    {
        Task<Guid> AddAsync(TaskModel task);

        Task<List<TaskModel>> GetAllAsync();

        Task<TaskModel?> GetByIdAsync(Guid id);

        Task<bool> UpdateAsync(TaskModel task);

        Task<bool> DeleteAsync(TaskModel task);
    }
}

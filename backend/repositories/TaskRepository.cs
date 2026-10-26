using backend.Data;
using backend.models;
using backend.repositories.interfaces;
using backend.services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _dbContext;

        private readonly IUserCurrent _userCurrent;

        public TaskRepository(AppDbContext dbContext, IUserCurrent userCurrent)
        {
            _dbContext = dbContext;
            _userCurrent = userCurrent;
        }

        public async Task<Guid> AddAsync(TaskModel task)
        {
            task.UserId = _userCurrent.Id;

            await _dbContext.AddAsync(task);
            await _dbContext.SaveChangesAsync();

            return task.Id;
        }

        public async Task<List<TaskModel>> GetAllAsync()
        {
            return await _dbContext.Tasks
                .AsNoTracking()
                .Include(t => t.User)
                .Where(t => t.UserId == _userCurrent.Id)
                .OrderByDescending(t => t.CreatedAt)
                .ThenByDescending(t => t.Priority)
                .ToListAsync();
        }

        public async Task<TaskModel?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Tasks
                .AsNoTracking()
                .Include(t => t.User)
                .Where(t => t.UserId == _userCurrent.Id)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> UpdateAsync(TaskModel task)
        {
            DateTime? completedAt = task.Status == enums.TaskStatus.Completed ? DateTime.Now : null;
            DateTime? cancelledAt = task.Status == enums.TaskStatus.Cancelled ? DateTime.Now : null;

            var rows = await _dbContext.Tasks
                .Where(t => t.UserId == _userCurrent.Id && t.Id == task.Id)
                .ExecuteUpdateAsync(t => t
                    .SetProperty(f => f.Title, task.Title)
                    .SetProperty(f => f.Description, task.Description)
                    .SetProperty(f => f.Priority, task.Priority)
                    .SetProperty(f => f.Status, task.Status)
                    .SetProperty(f => f.UpdatedAt, DateTime.Now)
                    .SetProperty(f => f.CompletedAt, completedAt)
                    .SetProperty(f => f.CancelledAt, cancelledAt)
                );

            return rows > 0;
        }

        public async Task<bool> DeleteAsync(TaskModel task)
        {
            var rows = await _dbContext.Tasks
                 .Where(t => t.UserId == _userCurrent.Id && t.Id == task.Id)
                 .ExecuteDeleteAsync();

            return rows > 0;
        }
    }
}

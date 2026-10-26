using backend.models;

namespace backend.repositories.interfaces
{
    public interface IUserRepository
    {
        Task<Guid> AddAsync(UserModel user);
        Task<UserModel?> GetByEmailAsync(string email);
    }
}

using backend.models.DTOs;

namespace backend.services.interfaces
{
    public interface IUserService
    {
        Task<Guid> AddAsync(UserAddDto userDto);
        Task<UserDto?> GetByEmailAsync(string email);
    }
}

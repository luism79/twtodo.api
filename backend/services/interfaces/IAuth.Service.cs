using backend.models.DTOs;

namespace backend.services.interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateTokenAsync(LoginInputDto loginInput);

        Task<AuthDto?> LoginAsync(LoginInputDto loginInput);
    }
}

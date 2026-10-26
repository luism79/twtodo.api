using backend.models;
using backend.models.DTOs;
using backend.repositories.interfaces;
using backend.services.exceptions;
using backend.services.interfaces;
using backend.shared;

namespace backend.services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository) => _repository = repository;

        public async Task<Guid> AddAsync(UserAddDto userDto)
        {
            if (await FindUserAsync(userDto.Email))
                throw new ServicesPostExceptions("Usuário já cadastrado");

            var newUser = await _repository.AddAsync(ConvertUserDtoToModel(userDto));
            return newUser;
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var user = await _repository.GetByEmailAsync(email);

            return ConvertUserModelToDto(user);
        }

        private async Task<bool> FindUserAsync(string email)
        {
            var user = await GetByEmailAsync(email);
            return (user != null);
        }

        private UserDto? ConvertUserModelToDto(UserModel? user)
        {
            if (user == null)
                return null;

            return new UserDto(
                Email: user.Email,
                Id: user.Id
            );
        }

        private UserModel ConvertUserDtoToModel(UserAddDto userDto)
        {
            return new UserModel() {
                Email = userDto.Email,
                Password = AppUtils.ConvertTextToHash(userDto.Password),
            };
        }
    }
}

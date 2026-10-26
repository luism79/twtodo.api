using backend.models;
using backend.models.DTOs;
using backend.repositories.interfaces;
using backend.services.exceptions;
using backend.services.interfaces;
using backend.shared;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        private readonly JwtSettings _jwtSettings;

        public AuthService(IUserRepository userRepository, JwtSettings jwtSettings)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
        }

        public async Task<string> GenerateTokenAsync(LoginInputDto loginInput)
        {
            var user = await FindUserAsync(loginInput);

            if (user == null)
                throw new ServicesAuthenticatedExceptions("Verificar! Usuário ou senha inválidos");
            
            return InternalGenerateToken(user.Id);
        }

        public async Task<AuthDto?> LoginAsync(LoginInputDto loginInput)
        {
            var user = await FindUserAsync(loginInput);

            if (user == null) 
                throw new ServicesAuthenticatedExceptions("Verificar! Usuário ou senha inválidos");

            return ConvertToAuthDto(user);
        }

        private async Task<UserModel?> FindUserAsync(LoginInputDto loginInput)
        {
            var user = await _userRepository.GetByEmailAsync(loginInput.Email);
            
            if (user is null || (user.Password != AppUtils.ConvertTextToHash(loginInput.Password)))
                return null;
            
            return user;
        }

        private AuthDto ConvertToAuthDto(UserModel user)
        {
            return new AuthDto(
                    UserId: user.Id,
                    AccessToken: InternalGenerateToken(user.Id),
                    ExpiresInMinutes: _jwtSettings.ExpiresInMinutes
                );
        }

        private string InternalGenerateToken(Guid userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var now = DateTime.UtcNow;


            //var descriptor = new SecurityTokenDescriptor
            //{
            //    Claims = new Dictionary<string, object>
            //    {
            //        ["Sub"] = UserMail
            //    },

            //    Issuer = _jwtSettings.Issuer,
            //    IssuedAt = now,
            //    Expires = now.AddMinutes(_jwtSettings.ExpiresInMinutes),
            //    SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            //};
            //return new JsonWebTokenHandler().CreateToken(descriptor);

            var claims = new[]
            {
                new Claim("sub", userId.ToString())
            };

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                notBefore: now,
                expires: now.AddMinutes(_jwtSettings.ExpiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

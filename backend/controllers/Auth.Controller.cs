using backend.models.DTOs;
using backend.services.interfaces;
using backend.shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginInputDto loginInput)
        {
           var auth = await _authService.LoginAsync(loginInput);            

            return StatusCode(
                StatusCodes.Status201Created,
                ApiResponse.Success(
                    auth,
                    "Autenticação realizada com sucesso"
                ));
        }
    }
}

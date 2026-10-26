using backend.models.DTOs;
using backend.services.exceptions;
using backend.services.interfaces;
using backend.shared;
using Microsoft.AspNetCore.Mvc;

[Route("users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service) => _service = service;

    /// <summary>
    /// Criar um usuário
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddAsync(UserAddDto user)
    {
        var id = await _service.AddAsync(user);

            return StatusCode(
                StatusCodes.Status201Created,
                ApiResponse.Success(new { id },
                    "Resgistro criado com sucesso"
            ));
    }

    /// <summary>
    /// Localizar usuário por e-mail
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="ServicesExceptions"></exception>
    [HttpGet("{email}")]
    public async Task<IActionResult> FindByEmailAsync(string email)
    {
        var user = await _service.GetByEmailAsync(email);

        if (user is null)
            throw new ServicesExceptions("Usuário não foi encontrado");

        return StatusCode(
            StatusCodes.Status200OK,
            ApiResponse.Success(user, "Usuário encontrado"
        ));
    }
}

using backend.models.DTOs;
using backend.services.exceptions;
using backend.services.interfaces;
using backend.shared;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers
{
    [Route("tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService) => this._taskService = taskService;

        [HttpPost]
        public async Task<IActionResult> AddAsync(TaskInputDto task)
        {
            var id = await _taskService.AddAsync(task);

            return StatusCode(
                StatusCodes.Status201Created,
                ApiResponse.Success(new { id },
                    "Resgistro criado com sucesso"
                ));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var tasks = await _taskService.GetAllAsync();

            return StatusCode(
                StatusCodes.Status200OK,
                ApiResponse.Success(
                    tasks,
                    "Registros listados com sucesso"
                ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ServicesExceptions("Argumento inválido");

            var task = await _taskService.GetByIdAsync(id);

            if (task is null)
                throw new ServicesExceptions("Registro não encontrado");

            return StatusCode(
                StatusCodes.Status200OK,
                ApiResponse.Success(
                    task,
                    "Registro localizado com sucesso"
                ));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, TaskInputDto taskUpt)
        {
            if (id == Guid.Empty)
                throw new ServicesExceptions("Argumento inválido");

            await _taskService.UpdateAsync(id, taskUpt);

            return StatusCode(
                StatusCodes.Status200OK,
                ApiResponse.Success(
                    new { id },
                    "Registro atualizado com sucesso"
                ));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ServicesExceptions("Argumento inválido");

            await _taskService.DeleteAsync(id);

            return StatusCode(
                StatusCodes.Status200OK,
                ApiResponse.Success(
                    new { id },
                    "Registro deletado com sucesso"
                ));

        }
    }
}

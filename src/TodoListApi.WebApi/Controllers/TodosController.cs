using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TodoListApi.Application.DTOs;
using TodoListApi.Application.Services;

namespace TodoListApi.WebApi.Controllers;

[ApiController]
[Route("api/todos")]
public class TodosController : ControllerBase
{
    private readonly TodoService _service;
    private readonly IValidator<CreateTodoItemDto> _createValidator;
    private readonly IValidator<UpdateTodoItemDto> _updateValidator;
    private readonly ILogger<TodosController> _logger;

    public TodosController(
        TodoService service,
        IValidator<CreateTodoItemDto> createValidator,
        IValidator<UpdateTodoItemDto> updateValidator,
        ILogger<TodosController> logger)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al obtener todas las tareas.");
            return StatusCode(500, "Ocurrió un error interno.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound(new { message = $"Tarea con Id {id} no encontrada." });
            return Ok(item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al obtener la tarea con Id {Id}.", id);
            return StatusCode(500, "Ocurrió un error interno.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoItemDto dto)
    {
        try
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al crear una tarea.");
            return StatusCode(500, "Ocurrió un error interno.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoItemDto dto)
    {
        try
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            var updated = await _service.UpdateAsync(id, dto);
            if (updated is null) return NotFound(new { message = $"Tarea con Id {id} no encontrada." });
            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al actualizar la tarea con Id {Id}.", id);
            return StatusCode(500, "Ocurrió un error interno.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = $"Tarea con Id {id} no encontrada." });
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al eliminar la tarea con Id {Id}.", id);
            return StatusCode(500, "Ocurrió un error interno.");
        }
    }
}
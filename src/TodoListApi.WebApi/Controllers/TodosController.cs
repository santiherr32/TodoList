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

    public TodosController(
        TodoService service,
        IValidator<CreateTodoItemDto> createValidator,
        IValidator<UpdateTodoItemDto> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    // GET /api/todos
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.GetAllAsync();
        return Ok(items);
    }

    // GET /api/todos/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) return NotFound(new { message = $"Tarea con Id {id} no encontrada." });
        return Ok(item);
    }

    // POST /api/todos
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoItemDto dto)
    {
        var validation = await _createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /api/todos/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoItemDto dto)
    {
        var validation = await _updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var updated = await _service.UpdateAsync(id, dto);
        if (updated is null) return NotFound(new { message = $"Tarea con Id {id} no encontrada." });

        return Ok(updated);
    }

    // DELETE /api/todos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound(new { message = $"Tarea con Id {id} no encontrada." });

        return NoContent();
    }
}
using Microsoft.Extensions.Logging;
using TodoListApi.Application.DTOs;
using TodoListApi.Domain.Entities;
using TodoListApi.Domain.Interfaces;

namespace TodoListApi.Application.Services;

public class TodoService
{
    private readonly ITodoRepository _repository;
    private readonly ILogger<TodoService> _logger;

    public TodoService(ITodoRepository repository, ILogger<TodoService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<TodoItemDto>> GetAllAsync()
    {
        _logger.LogInformation("Obteniendo todas las tareas.");
        var items = await _repository.GetAllAsync();
        _logger.LogInformation("Se encontraron {Count} tareas.", items.Count());
        return items.Select(MapToDto);
    }

    public async Task<TodoItemDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Buscando tarea con Id {Id}.", id);
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
            _logger.LogWarning("Tarea con Id {Id} no encontrada.", id);

        return item is null ? null : MapToDto(item);
    }

    public async Task<TodoItemDto> CreateAsync(CreateTodoItemDto dto)
    {
        _logger.LogInformation("Creando nueva tarea con título: {Title}.", dto.Title);
        var entity = new TodoItem
        {
            Title = dto.Title,
            Description = dto.Description,
            MaxCompletionDate = dto.MaxCompletionDate,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(entity);
        _logger.LogInformation("Tarea creada exitosamente con Id {Id}.", created.Id);
        return MapToDto(created);
    }

    public async Task<TodoItemDto?> UpdateAsync(int id, UpdateTodoItemDto dto)
    {
        _logger.LogInformation("Actualizando tarea con Id {Id}.", id);
        var entity = new TodoItem
        {
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted,
            MaxCompletionDate = dto.MaxCompletionDate
        };

        var updated = await _repository.UpdateAsync(id, entity);

        if (updated is null)
            _logger.LogWarning("No se pudo actualizar. Tarea con Id {Id} no encontrada.", id);
        else
            _logger.LogInformation("Tarea con Id {Id} actualizada exitosamente.", id);

        return updated is null ? null : MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Eliminando tarea con Id {Id}.", id);
        var result = await _repository.DeleteAsync(id);

        if (!result)
            _logger.LogWarning("No se pudo eliminar. Tarea con Id {Id} no encontrada.", id);
        else
            _logger.LogInformation("Tarea con Id {Id} eliminada exitosamente.", id);

        return result;
    }

    private static TodoItemDto MapToDto(TodoItem item) => new()
    {
        Id = item.Id,
        Title = item.Title,
        Description = item.Description,
        IsCompleted = item.IsCompleted,
        MaxCompletionDate = item.MaxCompletionDate,
        CreatedAt = item.CreatedAt
    };
}
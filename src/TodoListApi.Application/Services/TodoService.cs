using TodoListApi.Application.DTOs;
using TodoListApi.Domain.Entities;
using TodoListApi.Domain.Interfaces;

namespace TodoListApi.Application.Services;

public class TodoService
{
    private readonly ITodoRepository _repository;

    public TodoService(ITodoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TodoItemDto>> GetAllAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<TodoItemDto?> GetByIdAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item is null ? null : MapToDto(item);
    }

    public async Task<TodoItemDto> CreateAsync(CreateTodoItemDto dto)
    {
        var entity = new TodoItem
        {
            Title = dto.Title,
            Description = dto.Description,
            MaxCompletionDate = dto.MaxCompletionDate,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(entity);
        return MapToDto(created);
    }

    public async Task<TodoItemDto?> UpdateAsync(int id, UpdateTodoItemDto dto)
    {
        var entity = new TodoItem
        {
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted,
            MaxCompletionDate = dto.MaxCompletionDate
        };

        var updated = await _repository.UpdateAsync(id, entity);
        return updated is null ? null : MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
        => await _repository.DeleteAsync(id);

    // Mapeo manual (sin AutoMapper para mantenerlo simple)
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
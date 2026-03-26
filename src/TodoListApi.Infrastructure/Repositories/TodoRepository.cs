using Microsoft.EntityFrameworkCore;
using TodoListApi.Domain.Entities;
using TodoListApi.Domain.Interfaces;
using TodoListApi.Infrastructure.Persistence;

namespace TodoListApi.Infrastructure.Repositories;

// Implementación concreta del contrato definido en Domain
public class TodoRepository : ITodoRepository
{
    private readonly AppDbContext _context;

    public TodoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync()
        => await _context.TodoItems.ToListAsync();

    public async Task<TodoItem?> GetByIdAsync(int id)
        => await _context.TodoItems.FindAsync(id);

    public async Task<TodoItem> CreateAsync(TodoItem item)
    {
        _context.TodoItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<TodoItem?> UpdateAsync(int id, TodoItem item)
    {
        var existing = await _context.TodoItems.FindAsync(id);
        if (existing is null) return null;

        existing.Title = item.Title;
        existing.Description = item.Description;
        existing.IsCompleted = item.IsCompleted;
        existing.MaxCompletionDate = item.MaxCompletionDate;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.TodoItems.FindAsync(id);
        if (existing is null) return false;

        _context.TodoItems.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
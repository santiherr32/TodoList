using TodoListApi.Domain.Entities;

namespace TodoListApi.Domain.Interfaces;

// Contrato que define QUÉ operaciones existen.
// Infrastructure lo implementará, Application lo usará.
public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> GetAllAsync();
    Task<TodoItem?> GetByIdAsync(int id);
    Task<TodoItem> CreateAsync(TodoItem item);
    Task<TodoItem?> UpdateAsync(int id, TodoItem item);
    Task<bool> DeleteAsync(int id);
}
namespace TodoListApi.Application.DTOs;

// Lo que la API devuelve al cliente
public class TodoItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime MaxCompletionDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
namespace TodoListApi.Application.DTOs;

// Lo que el cliente envía al EDITAR
public class UpdateTodoItemDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime MaxCompletionDate { get; set; }
}
namespace TodoListApi.Application.DTOs;

// Lo que el cliente envía al CREAR
public class CreateTodoItemDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime MaxCompletionDate { get; set; }
}
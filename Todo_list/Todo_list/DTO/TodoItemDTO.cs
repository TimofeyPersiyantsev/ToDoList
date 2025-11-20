namespace Todo_list.DTO;

/// <summary>
/// DTO для отображения задачи
/// </summary>
public class TodoItemDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public int TodoListId { get; set; }
    public string TodoListTitle { get; set; } = string.Empty;
    public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.Today && !IsCompleted;
}

/// <summary>
/// DTO для создания новой задачи
/// </summary>
public class CreateTodoItemDTO
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public int TodoListId { get; set; }
}

/// <summary>
/// DTO для обновления задачи
/// </summary>
public class UpdateTodoItemDTO
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
}
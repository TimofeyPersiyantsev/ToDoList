namespace Todo_list.DTO;

/// <summary>
/// DTO для отображения списка дел (краткая информация)
/// </summary>
public class TodoListDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public int CompletedCount { get; set; }
    public double CompletionPercentage => ItemCount > 0 ? (CompletedCount * 100.0) / ItemCount : 0;
}

/// <summary>
/// DTO для создания нового списка дел
/// </summary>
public class CreateTodoListDTO
{
    public string Title { get; set; } = string.Empty;
}

/// <summary>
/// DTO для обновления списка дел
/// </summary>
public class UpdateTodoListDTO
{
    public string Title { get; set; } = string.Empty;
}

/// <summary>
/// DTO для отображения детальной информации о списке дел с его задачами
/// </summary>
public class TodoListDetailDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<TodoItemDTO> Items { get; set; } = new();
    public int TotalItems => Items.Count;
    public int CompletedItems => Items.Count(i => i.IsCompleted);
}
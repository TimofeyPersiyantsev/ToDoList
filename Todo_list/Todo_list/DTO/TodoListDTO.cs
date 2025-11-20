namespace Todo_list.DTO;

public class TodoListDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public int CompletedCount { get; set; }
}

public class TodoListDetailDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<TodoItemDTO> Items { get; set; } = new();
}
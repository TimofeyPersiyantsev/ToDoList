namespace Todo_list.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } // Описание задачи
        public bool IsCompleted { get; set; } = false; // Статус выполнения
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public int TodoListId { get; set; }
        public TodoList TodoList { get; set; } = null!;
    }
}

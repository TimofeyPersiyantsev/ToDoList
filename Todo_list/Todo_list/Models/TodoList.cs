namespace Todo_list.Models
{

    public class TodoList
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ItemCount { get; set; }
        public int CompletionCount { get; set; }
        public double CompletionPercentage { get; set; }
        public List<TodoItem> Items { get; set; } = new();

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}

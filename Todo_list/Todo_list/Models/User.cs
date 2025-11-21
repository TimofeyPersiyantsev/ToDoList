// Models/User.cs
using Todo_list.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Связь с TodoList (если нужно привязать списки к пользователям)
    public List<TodoList> TodoLists { get; set; } = new();
}
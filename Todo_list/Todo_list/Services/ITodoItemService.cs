namespace Todo_list.Services;
using Todo_list.DTO;

/// <summary>
/// Сервис для работы с задачами
/// </summary>
public interface ITodoItemService
{
    /// <summary>
    /// Получить все задачи
    /// </summary>
    Task<IEnumerable<TodoItemDTO>> GetAllItemsAsync();

    /// <summary>
    /// Получить задачу по ID
    /// </summary>
    Task<TodoItemDTO?> GetItemByIdAsync(int id);

    /// <summary>
    /// Получить задачи по ID списка
    /// </summary>
    Task<IEnumerable<TodoItemDTO>> GetItemsByListIdAsync(int listId);

    /// <summary>
    /// Создать новую задачу
    /// </summary>
    Task<TodoItemDTO> CreateItemAsync(CreateTodoItemDTO createDto);

    /// <summary>
    /// Обновить задачу
    /// </summary>
    Task<bool> UpdateItemAsync(int id, UpdateTodoItemDTO updateDto);

    /// <summary>
    /// Переключить статус выполнения задачи
    /// </summary>
    Task<bool> ToggleItemCompletionAsync(int id);

    /// <summary>
    /// Удалить задачу
    /// </summary>
    Task<bool> DeleteItemAsync(int id);

    /// <summary>
    /// Получить просроченные задачи
    /// </summary>
    Task<IEnumerable<TodoItemDTO>> GetOverdueItemsAsync();

    /// <summary>
    /// Получить задачи на сегодня
    /// </summary>
    Task<IEnumerable<TodoItemDTO>> GetTodayItemsAsync();
}
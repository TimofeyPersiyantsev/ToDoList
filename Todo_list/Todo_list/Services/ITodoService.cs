namespace Todo_list.Services;
using Todo_list.DTO;

/// <summary>
/// Сервис для работы со списками дел
/// </summary>
public interface ITodoService
{
    /// <summary>
    /// Получить все списки дел с краткой информацией
    /// </summary>
    Task<IEnumerable<TodoListDTO>> GetAllListsAsync();

    /// <summary>
    /// Получить детальную информацию о списке дел по ID
    /// </summary>
    Task<TodoListDetailDTO?> GetListByIdAsync(int id);

    /// <summary>
    /// Создать новый список дел
    /// </summary>
    Task<TodoListDTO> CreateListAsync(CreateTodoListDTO createDto);

    /// <summary>
    /// Обновить список дел
    /// </summary>
    Task<bool> UpdateListAsync(int id, UpdateTodoListDTO updateDto);

    /// <summary>
    /// Удалить список дел
    /// </summary>
    Task<bool> DeleteListAsync(int id);

    /// <summary>
    /// Получить статистику по всем спискам
    /// </summary>
    Task<object> GetStatisticsAsync();
}
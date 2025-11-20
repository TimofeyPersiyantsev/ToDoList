namespace Todo_list.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Todo_list.Data;
using Todo_list.DTO;
using Todo_list.Models;

/// <summary>
/// Сервис для работы с задачами
/// </summary>
public class TodoItemService : ITodoItemService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Конструктор сервиса
    /// </summary>
    public TodoItemService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить все задачи
    /// </summary>
    public async Task<IEnumerable<TodoItemDTO>> GetAllItemsAsync()
    {
        var items = await _context.TodoItems
            .Include(ti => ti.TodoList)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TodoItemDTO>>(items);
    }

    /// <summary>
    /// Получить задачу по ID
    /// </summary>
    public async Task<TodoItemDTO?> GetItemByIdAsync(int id)
    {
        var item = await _context.TodoItems
            .Include(ti => ti.TodoList)
            .FirstOrDefaultAsync(ti => ti.Id == id);

        return _mapper.Map<TodoItemDTO>(item);
    }

    /// <summary>
    /// Получить задачи по ID списка
    /// </summary>
    public async Task<IEnumerable<TodoItemDTO>> GetItemsByListIdAsync(int listId)
    {
        var items = await _context.TodoItems
            .Where(ti => ti.TodoListId == listId)
            .Include(ti => ti.TodoList)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TodoItemDTO>>(items);
    }

    /// <summary>
    /// Создать новую задачу
    /// </summary>
    public async Task<TodoItemDTO> CreateItemAsync(CreateTodoItemDTO createDto)
    {
        // Проверяем существование списка
        var list = await _context.TodoLists.FindAsync(createDto.TodoListId);
        if (list == null)
            throw new ArgumentException("TodoList not found");

        var item = _mapper.Map<TodoItem>(createDto);

        _context.TodoItems.Add(item);
        await _context.SaveChangesAsync();

        // Загружаем связанные данные для маппинга
        await _context.Entry(item).Reference(ti => ti.TodoList).LoadAsync();

        return _mapper.Map<TodoItemDTO>(item);
    }

    /// <summary>
    /// Обновить задачу
    /// </summary>
    public async Task<bool> UpdateItemAsync(int id, UpdateTodoItemDTO updateDto)
    {
        var item = await _context.TodoItems
            .Include(ti => ti.TodoList)
            .FirstOrDefaultAsync(ti => ti.Id == id);

        if (item == null) return false;

        _mapper.Map(updateDto, item);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Переключить статус выполнения задачи
    /// </summary>
    public async Task<bool> ToggleItemCompletionAsync(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null) return false;

        item.IsCompleted = !item.IsCompleted;
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Удалить задачу
    /// </summary>
    public async Task<bool> DeleteItemAsync(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null) return false;

        _context.TodoItems.Remove(item);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Получить просроченные задачи
    /// </summary>
    public async Task<IEnumerable<TodoItemDTO>> GetOverdueItemsAsync()
    {
        var items = await _context.TodoItems
            .Where(ti => ti.DueDate.HasValue &&
                        ti.DueDate.Value < DateTime.Today &&
                        !ti.IsCompleted)
            .Include(ti => ti.TodoList)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TodoItemDTO>>(items);
    }

    /// <summary>
    /// Получить задачи на сегодня
    /// </summary>
    public async Task<IEnumerable<TodoItemDTO>> GetTodayItemsAsync()
    {
        var today = DateTime.Today;
        var items = await _context.TodoItems
            .Where(ti => ti.DueDate.HasValue &&
                        ti.DueDate.Value.Date == today)
            .Include(ti => ti.TodoList)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TodoItemDTO>>(items);
    }
}

namespace Todo_list.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Todo_list.Data;
using Todo_list.DTO;
using Todo_list.Models;
/// <summary>
/// Сервис для работы со списками дел
/// </summary>
public class TodoService : ITodoService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Конструктор сервиса
    /// </summary>
    public TodoService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить все списки дел с краткой информацией
    /// </summary>
    public async Task<IEnumerable<TodoListDTO>> GetAllListsAsync()
    {
        var lists = await _context.TodoLists
            .Include(tl => tl.Items)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TodoListDTO>>(lists);
    }

    /// <summary>
    /// Получить детальную информацию о списке дел по ID
    /// </summary>
    public async Task<TodoListDetailDTO?> GetListByIdAsync(int id)
    {
        var list = await _context.TodoLists
            .Include(tl => tl.Items)
            .FirstOrDefaultAsync(tl => tl.Id == id);

        return _mapper.Map<TodoListDetailDTO>(list);
    }

    /// <summary>
    /// Создать новый список дел
    /// </summary>
    public async Task<TodoListDTO> CreateListAsync(CreateTodoListDTO createDto)
    {
        var list = _mapper.Map<TodoList>(createDto);

        _context.TodoLists.Add(list);
        await _context.SaveChangesAsync();

        return _mapper.Map<TodoListDTO>(list);
    }

    /// <summary>
    /// Обновить список дел
    /// </summary>
    public async Task<bool> UpdateListAsync(int id, UpdateTodoListDTO updateDto)
    {
        var list = await _context.TodoLists.FindAsync(id);
        if (list == null) return false;

        _mapper.Map(updateDto, list);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Удалить список дел
    /// </summary>
    public async Task<bool> DeleteListAsync(int id)
    {
        var list = await _context.TodoLists.FindAsync(id);
        if (list == null) return false;

        _context.TodoLists.Remove(list);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Получить статистику по всем спискам
    /// </summary>
    public async Task<object> GetStatisticsAsync()
    {
        var lists = await _context.TodoLists
            .Include(tl => tl.Items)
            .ToListAsync();

        var totalLists = lists.Count;
        var totalItems = lists.Sum(tl => tl.Items.Count);
        var completedItems = lists.Sum(tl => tl.Items.Count(i => i.IsCompleted));
        var overdueItems = lists.Sum(tl => tl.Items.Count(i => i.DueDate.HasValue && i.DueDate.Value < DateTime.Today && !i.IsCompleted));

        return new
        {
            TotalLists = totalLists,
            TotalItems = totalItems,
            CompletedItems = completedItems,
            OverdueItems = overdueItems,
            CompletionRate = totalItems > 0 ? (completedItems * 100.0) / totalItems : 0
        };
    }
}
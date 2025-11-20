using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo_list.Data;
using Todo_list.DTO;
using Todo_list.Models;
using Todo_list.Services;
namespace Todo_list.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoItemsController : ControllerBase
{
    private readonly ITodoItemService _todoItemService;

    /// <summary>
    /// Конструктор контроллера задач
    /// </summary>
    public TodoItemsController(ITodoItemService todoItemService)
    {
        _todoItemService = todoItemService;
    }

    /// <summary>
    /// Получить все задачи
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
        var items = await _todoItemService.GetAllItemsAsync();
        return Ok(items);
    }

    /// <summary>
    /// Получить задачу по ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(int id)
    {
        var item = await _todoItemService.GetItemByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    /// <summary>
    /// Получить задачи по ID списка
    /// </summary>
    [HttpGet("by-list/{listId}")]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItemsByList(int listId)
    {
        var items = await _todoItemService.GetItemsByListIdAsync(listId);
        return Ok(items);
    }

    /// <summary>
    /// Получить просроченные задачи
    /// </summary>
    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetOverdueItems()
    {
        var items = await _todoItemService.GetOverdueItemsAsync();
        return Ok(items);
    }

    /// <summary>
    /// Получить задачи на сегодня
    /// </summary>
    [HttpGet("today")]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodayItems()
    {
        var items = await _todoItemService.GetTodayItemsAsync();
        return Ok(items);
    }

    /// <summary>
    /// Создать новую задачу
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(CreateTodoItemDTO createDto)
    {
        try
        {
            var item = await _todoItemService.CreateItemAsync(createDto);
            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Обновить задачу
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(int id, UpdateTodoItemDTO updateDto)
    {
        var result = await _todoItemService.UpdateItemAsync(id, updateDto);
        if (!result) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Переключить статус выполнения задачи
    /// </summary>
    [HttpPatch("{id}/toggle")]
    public async Task<IActionResult> ToggleTodoItem(int id)
    {
        var result = await _todoItemService.ToggleItemCompletionAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Удалить задачу
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        var result = await _todoItemService.DeleteItemAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
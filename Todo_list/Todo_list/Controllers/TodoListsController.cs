namespace Todo_list.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo_list.Data;
using Todo_list.DTO;
using Todo_list.Models;
using Todo_list.Services;

[ApiController]
[Route("api/[controller]")]
public class TodoListsController : ControllerBase
{
    private readonly ITodoService _todoService;

    /// <summary>
    /// Конструктор контроллера
    /// </summary>
    public TodoListsController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    /// <summary>
    /// Получить все списки дел
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoListDTO>>> GetTodoLists()
    {
        var lists = await _todoService.GetAllListsAsync();
        return Ok(lists);
    }

    /// <summary>
    /// Получить список дел по ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoListDetailDTO>> GetTodoList(int id)
    {
        var list = await _todoService.GetListByIdAsync(id);
        if (list == null) return NotFound();
        return Ok(list);
    }

    /// <summary>
    /// Создать новый список дел
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TodoListDTO>> PostTodoList(CreateTodoListDTO createDto)
    {
        var list = await _todoService.CreateListAsync(createDto);
        return CreatedAtAction(nameof(GetTodoList), new { id = list.Id }, list);
    }

    /// <summary>
    /// Обновить список дел
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoList(int id, UpdateTodoListDTO updateDto)
    {
        var result = await _todoService.UpdateListAsync(id, updateDto);
        if (!result) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Удалить список дел
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoList(int id)
    {
        var result = await _todoService.DeleteListAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Получить статистику по спискам дел
    /// </summary>
    [HttpGet("statistics")]
    public async Task<ActionResult<object>> GetStatistics()
    {
        var statistics = await _todoService.GetStatisticsAsync();
        return Ok(statistics);
    }
} 

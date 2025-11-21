using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo_list.Data;
using Todo_list.Models;

namespace Todo_list.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly AppDbContext _context;

    public TestController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("check-db")]
    public async Task<IActionResult> CheckDatabase()
    {
        try
        {
            // Проверяем таблицы
            var listsCount = await _context.TodoLists.CountAsync();
            var itemsCount = await _context.TodoItems.CountAsync();

            return Ok(new
            {
                Success = true,
                ListsCount = listsCount,
                ItemsCount = itemsCount,
                Message = "Database is working!"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Success = false,
                Error = ex.Message,
                Message = "Database error"
            });
        }
    }

    [HttpPost("add-test-data")]
    public async Task<IActionResult> AddTestData()
    {
        try
        {
            var list = new TodoList { Title = "Тест из API" };
            _context.TodoLists.Add(list);
            await _context.SaveChangesAsync();

            var item = new TodoItem
            {
                Title = "Тестовая задача из API",
                TodoListId = list.Id
            };
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                ListId = list.Id,
                ItemId = item.Id,
                Message = "Test data added successfully!"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Success = false,
                Error = ex.Message
            });
        }
    }
}
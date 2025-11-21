using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Todo_list.Data;
using Todo_list.Models;

namespace Todo_list.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Требуем авторизацию для всех методов
    public class TodoListController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TodoListController> _logger;

        public TodoListController(AppDbContext context, ILogger<TodoListController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Вспомогательный метод для получения ID текущего пользователя
        private int GetCurrentUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User not authenticated");

            return int.Parse(userId);
        }

        /// <summary>
        /// Получить все списки задач текущего пользователя
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoList>>> GetTodoLists()
        {
            var userId = GetCurrentUserId();

            var todoLists = await _context.TodoLists
                .Where(tl => tl.UserId == userId)
                .Include(tl => tl.Items)
                .ToListAsync();

            _logger.LogInformation("User {UserId} retrieved {Count} todo lists", userId, todoLists.Count);

            return Ok(todoLists);
        }

        /// <summary>
        /// Получить конкретный список задач по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoList>> GetTodoList(int id)
        {
            var userId = GetCurrentUserId();

            var todoList = await _context.TodoLists
                .Include(tl => tl.Items)
                .FirstOrDefaultAsync(tl => tl.Id == id && tl.UserId == userId);

            if (todoList == null)
            {
                _logger.LogWarning("User {UserId} attempted to access non-existent todo list {TodoListId}", userId, id);
                return NotFound(new { error = "Todo list not found" });
            }

            return Ok(todoList);
        }

        /// <summary>
        /// Создать новый список задач
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TodoList>> CreateTodoList([FromBody] TodoList todoList)
        {
            var userId = GetCurrentUserId();

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Invalid todo list data" });

            // Привязываем список к текущему пользователю
            todoList.UserId = userId;

            _context.TodoLists.Add(todoList);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {UserId} created new todo list {TodoListId}", userId, todoList.Id);

            return CreatedAtAction(nameof(GetTodoList), new { id = todoList.Id }, todoList);
        }

        /// <summary>
        /// Обновить список задач
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoList(int id, [FromBody] TodoList updatedTodoList)
        {
            var userId = GetCurrentUserId();

            var existingTodoList = await _context.TodoLists
                .FirstOrDefaultAsync(tl => tl.Id == id && tl.UserId == userId);

            if (existingTodoList == null)
                return NotFound(new { error = "Todo list not found" });

            existingTodoList.Title = updatedTodoList.Title;
            // Обновляем другие свойства по необходимости

            await _context.SaveChangesAsync();

            _logger.LogInformation("User {UserId} updated todo list {TodoListId}", userId, id);

            return NoContent();
        }

        /// <summary>
        /// Удалить список задач
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoList(int id)
        {
            var userId = GetCurrentUserId();

            var todoList = await _context.TodoLists
                .FirstOrDefaultAsync(tl => tl.Id == id && tl.UserId == userId);

            if (todoList == null)
                return NotFound(new { error = "Todo list not found" });

            _context.TodoLists.Remove(todoList);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {UserId} deleted todo list {TodoListId}", userId, id);

            return NoContent();
        }
    }
}
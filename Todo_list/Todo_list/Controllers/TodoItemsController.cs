using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo_list.Data;
using Todo_list.DTO;
using Todo_list.Models;
using Todo_list.Data;
using Todo_list.DTO;
using Todo_list.Models;

namespace Todo_list.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoItemsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TodoItemsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
        var items = await _context.TodoItems
            .Include(ti => ti.TodoList)
            .Select(ti => new TodoItemDTO
            {
                Id = ti.Id,
                Title = ti.Title,
                Description = ti.Description,
                IsCompleted = ti.IsCompleted,
                CreatedAt = ti.CreatedAt,
                DueDate = ti.DueDate,
                TodoListId = ti.TodoListId,
                TodoListTitle = ti.TodoList.Title
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(int id)
    {
        var todoItem = await _context.TodoItems
            .Include(ti => ti.TodoList)
            .FirstOrDefaultAsync(ti => ti.Id == id);

        if (todoItem == null)
        {
            return NotFound();
        }

        var todoItemDTO = new TodoItemDTO
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            IsCompleted = todoItem.IsCompleted,
            CreatedAt = todoItem.CreatedAt,
            DueDate = todoItem.DueDate,
            TodoListId = todoItem.TodoListId,
            TodoListTitle = todoItem.TodoList.Title
        };

        return todoItemDTO;
    }

    [HttpGet("by-list/{listId}")]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItemsByList(int listId)
    {
        var items = await _context.TodoItems
            .Where(ti => ti.TodoListId == listId)
            .Include(ti => ti.TodoList)
            .Select(ti => new TodoItemDTO
            {
                Id = ti.Id,
                Title = ti.Title,
                Description = ti.Description,
                IsCompleted = ti.IsCompleted,
                CreatedAt = ti.CreatedAt,
                DueDate = ti.DueDate,
                TodoListId = ti.TodoListId,
                TodoListTitle = ti.TodoList.Title
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(CreateTodoItemDTO createDTO)
    {
        // Проверяем существование списка
        var todoList = await _context.TodoLists.FindAsync(createDTO.TodoListId);
        if (todoList == null)
        {
            return BadRequest("TodoList not found");
        }

        var todoItem = new TodoItem
        {
            Title = createDTO.Title,
            Description = createDTO.Description,
            DueDate = createDTO.DueDate,
            TodoListId = createDTO.TodoListId,
            CreatedAt = DateTime.UtcNow
        };

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        var resultDTO = new TodoItemDTO
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            IsCompleted = todoItem.IsCompleted,
            CreatedAt = todoItem.CreatedAt,
            DueDate = todoItem.DueDate,
            TodoListId = todoItem.TodoListId,
            TodoListTitle = todoList.Title
        };

        return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, resultDTO);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(int id, UpdateTodoItemDTO updateDTO)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.Title = updateDTO.Title;
        todoItem.Description = updateDTO.Description;
        todoItem.IsCompleted = updateDTO.IsCompleted;
        todoItem.DueDate = updateDTO.DueDate;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodoItemExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPatch("{id}/toggle")]
    public async Task<IActionResult> ToggleTodoItem(int id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.IsCompleted = !todoItem.IsCompleted;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemExists(int id)
    {
        return _context.TodoItems.Any(e => e.Id == id);
    }
}
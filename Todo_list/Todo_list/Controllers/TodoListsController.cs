namespace Todo_list.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo_list.Data;
using Todo_list.DTO;
using Todo_list.Models;
using Todo_list.Data;
using Todo_list.DTO;
using Todo_list.Models;

[ApiController]
[Route("api/[controller]")]
public class TodoListsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TodoListsController(AppDbContext context)
    {
        _context = context;
    }

   [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoListDTO>>> GetTodoLists()
    {
        var lists = await _context.TodoLists
            .Include(tl => tl.Items)
            .Select(tl => new TodoListDTO
            {
                Id = tl.Id,
                Title = tl.Title,
                ItemCount = tl.Items.Count,
                CompletedCount = tl.Items.Count(i => i.IsCompleted)
            })
            .ToListAsync();

        return Ok(lists);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoListDetailDTO>> GetTodoList(int id)
    {
        var todoList = await _context.TodoLists
            .Include(tl => tl.Items)
            .FirstOrDefaultAsync(tl => tl.Id == id);

        if (todoList == null)
        {
            return NotFound();
        }

        var todoListDTO = new TodoListDetailDTO
        {
            Id = todoList.Id,
            Title = todoList.Title,
            Items = todoList.Items.Select(item => new TodoItemDTO
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                IsCompleted = item.IsCompleted,
                CreatedAt = item.CreatedAt,
                DueDate = item.DueDate,
                TodoListId = item.TodoListId,
                TodoListTitle = todoList.Title
            }).ToList()
        };

        return todoListDTO;
    }

    [HttpPost]
    public async Task<ActionResult<TodoListDTO>> PostTodoList(TodoListDTO todoListDTO)
    {
        var todoList = new TodoList
        {
            Title = todoListDTO.Title
        };

        _context.TodoLists.Add(todoList);
        await _context.SaveChangesAsync();

        var resultDTO = new TodoListDTO
        {
            Id = todoList.Id,
            Title = todoList.Title,
            ItemCount = 0,
            CompletedCount = 0
        };

        return CreatedAtAction(nameof(GetTodoList), new { id = todoList.Id }, resultDTO);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoList(int id, TodoListDTO todoListDTO)
    {
        if (id != todoListDTO.Id)
        {
            return BadRequest();
        }

        var todoList = await _context.TodoLists.FindAsync(id);
        if (todoList == null)
        {
            return NotFound();
        }

        todoList.Title = todoListDTO.Title;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodoListExists(id))
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoList(int id)
    {
        var todoList = await _context.TodoLists.FindAsync(id);
        if (todoList == null)
        {
            return NotFound();
        }

        _context.TodoLists.Remove(todoList);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoListExists(int id)
    {
        return _context.TodoLists.Any(e => e.Id == id);
    }
} 

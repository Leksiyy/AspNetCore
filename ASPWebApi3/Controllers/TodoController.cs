using ASPWebApi3.Data;
using ASPWebApi3.Models;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPWebApi3.Controllers
{
[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly ApplicationContext _context;
 
    public TodoController(ApplicationContext context)
    {
        _context = context;
    }
 
    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemDTO todoDTO)
    {
        var todoItem = new TodoItem
        {
            IsComplete = todoDTO.IsComplete,
            Name = todoDTO.Name
        };
 
        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();
 
        return CreatedAtAction(
            nameof(GetTodoItem),
            new { id = todoItem.Id },
            ItemToDTO(todoItem));
    }
 
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }
        return ItemToDTO(todoItem);
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItem()
    {
        return await _context.TodoItems
        .Select(x => ItemToDTO(x))
        .ToListAsync();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
    {
        if (id != todoDTO.Id)
        {
            return BadRequest();
        }
 
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }
 
        todoItem.Name = todoDTO.Name;
        todoItem.IsComplete = todoDTO.IsComplete;
 
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
        {
            return NotFound();
        }
 
        return NoContent();
    }
 
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
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
 
    private bool TodoItemExists(long id)
    {
        return _context.TodoItems.Any(e => e.Id == id);
    }
 
    private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
    new TodoItemDTO
    {
        Id = todoItem.Id,
        Name = todoItem.Name,
        IsComplete = todoItem.IsComplete
    };
 
}
}

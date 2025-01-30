using homework18.Data;
using homework18.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace homework18.Pages;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public List<ToDo> ToDos { get; set; } = new();
    
    public IndexModel(ApplicationDbContext context ,UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    public PageResult OnGet()
    {
        var user = _userManager.GetUserId(User);
        ToDos = _context.ToDos.Where(e => e.UserId == user).ToList();
        return Page();
    }

    public async Task<IActionResult> OnPostEditAsync(int id, string newTitle)
    {
        var todo = await _context.ToDos.FindAsync(id);
        if (todo != null && !string.IsNullOrWhiteSpace(newTitle))
        {
            todo.Title = newTitle;
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
    }

    public async Task<IActionResult?> OnPostDoneAsync(int id)
    {
        Console.WriteLine(id);
        var todo = await _context.ToDos.FindAsync(id);
        if (todo != null)
        {
            todo.IsDone = !todo.IsDone;
            await _context.SaveChangesAsync();
            return RedirectToPage("/Index");
        }
        else
        {
            return BadRequest();
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var todo = await _context.ToDos.FindAsync(id);
        if (todo != null)
        {
            _context.ToDos.Remove(todo);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
    }

    public IActionResult OnPostAdd(ToDo toDo)
    {
        var userId = _userManager.GetUserId(User);
    
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        toDo.UserId = userId;
        _context.ToDos.Add(toDo);
        _context.SaveChanges();

        return RedirectToPage("/Index");
    }

}
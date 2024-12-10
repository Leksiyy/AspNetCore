using homework10.Data;
using homework10.Models;
using Microsoft.AspNetCore.Mvc;

namespace homework10.Controllers;

public class CommentController : Controller
{
    private readonly ApplicationContext _context;

    public CommentController(ApplicationContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult AddComment(Comment comment)
    {
        _context.Comments.Add(comment);
        _context.SaveChanges();
        
        var viewModel = new IndexViewModel
        {
            Books = _context.Books.ToList(),
            Comments = _context.Comments.ToList(),
        };
        
        return RedirectToAction("Index", "Home", viewModel);
    }
}
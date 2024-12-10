using System.Diagnostics;
using homework10.Data;
using Microsoft.AspNetCore.Mvc;
using homework10.Models;
using Microsoft.Extensions.Logging;

namespace homework10.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationContext _context;

    public HomeController(ApplicationContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        var viewModel = new IndexViewModel
        {
            Books = _context.Books.ToList(),
            Comments = _context.Comments.ToList(),
        };
        
        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using homework12.Models;
using homework12.Services;
using homework12.ViewModels;

namespace homework12.Controllers;

public class HomeController : Controller
{
    private readonly NewsService _newsService;

    public HomeController(NewsService newsService)
    {
        _newsService = newsService;
    }
    public IActionResult Index()
    {
        
        bool theme = bool.Parse(HttpContext.Request.Cookies["theme"] ?? "true");
        
        return View(new NewsBoolViewModel{ news = _newsService.GetNews(), theme = theme});
    }
}
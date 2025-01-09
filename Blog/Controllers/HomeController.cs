using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;

namespace Blog.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
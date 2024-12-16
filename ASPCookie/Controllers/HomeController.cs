using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASPCookie.Models;

namespace ASPCookie.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        HttpContext.Items["userName"] = "Alex";
        return View();
    }

    [HttpPost]
    public IActionResult Index(string userName)
    {
        return RedirectToAction(nameof(Index));
    }
}
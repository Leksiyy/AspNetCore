using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASPAuth.Models;
using Microsoft.AspNetCore.Authorization;

namespace ASPAuth.Controllers;

public class HomeController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
}
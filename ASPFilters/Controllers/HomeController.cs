using System.Diagnostics;
using ASPFilters.Filters;
using Microsoft.AspNetCore.Mvc;
using ASPFilters.Models;

namespace ASPFilters.Controllers;

public class HomeController : Controller
{
    [SimpleResourceFilter]
    public IActionResult Index()
    {
        return View();
    }
}
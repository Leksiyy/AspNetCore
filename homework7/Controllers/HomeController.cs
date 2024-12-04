using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using homework7.Models;

namespace homework7.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    private Dictionary<string, int> CurrencyDictionary { get; set; } = new Dictionary<string, int>()
    {
        {"UAH", 1},
        {"USD", 42},
        {"RUB", 2},
    }; // но можно и получить из API

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.Currency = CurrencyDictionary.Keys.ToList();
        ViewBag.Result = TempData["Result"];
        return View();
    }

    [HttpPost]
    public IActionResult ProcessForm(int Count, string Name1, string Name2)
    {
        TempData["Result"] = CurrencyDictionary[Name1] * Count / CurrencyDictionary[Name2];
        return RedirectToAction("Index");
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
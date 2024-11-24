using AspNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMVC.Controllers;

public class CompanyController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public string GetUser(string name, int age)
    {
        return $"Name - {name}, Age - {age}";
    }

    [HttpPost]

    public string GetUser2(Person person)
    {
        return $"Name - {person.name}, Age - {person.age}";
    }

    public string GetUser3()
    {
        string name = Request.Query["name"];
        int age = int.Parse(Request.Query["age"]);
        return $"Name - {name}, Age - {age}";
    }
}
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASPValidation.Models;

namespace ASPValidation.Controllers;

public class HomeController : Controller
{
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Craete(Person person)
    {
        if (ModelState.IsValid)
        {
            return Content($"{person.ToString()}");
        }
        else
        {
            return View(person);
        }
    }
}
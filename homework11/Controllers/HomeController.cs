using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using homework11.Models;

namespace homework11.Controllers;

public class HomeController : Controller
{
    public List<string> UserNamesList = new List<string>{"Hello", "World"}; // db
    
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ValidateUserName(string UserName)
    {
        if (UserNamesList.Contains(UserName))
        {
            return View(true);
        }
        
        return View($"Имя пользователя {UserName} уже занято.");
    }
}
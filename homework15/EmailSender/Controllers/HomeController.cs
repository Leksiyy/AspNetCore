using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmailSender.Models;
using EmailSender.ViewModel;
using EmailService;

namespace EmailSender.Controllers;

public class HomeController : Controller
{
    private readonly IEmailSender _emailSender;
    private readonly MessageCheckService _checkService;
    
    public HomeController(IEmailSender emailSender, MessageCheckService checkService)
    {
        _emailSender = emailSender;
        _checkService = checkService;
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult Index(MessageViewModel model)
    {
        if (ModelState.IsValid)
        {
            DateTime currentDate = DateTime.Now;
            Message mess = new Message
            {
                Subject = model.Subject,
                Content = model.Content,
                Email = model.Email,
            };
            switch (model.SendDate)
            {
                case "6m":
                    mess.SendDate = currentDate.AddMonths(6);
                    break;
                case "1y":
                    mess.SendDate = currentDate.AddYears(1);
                    break;
                case "3y":
                    mess.SendDate = currentDate.AddYears(3);
                    break;
                case "5y":
                    mess.SendDate = currentDate.AddYears(5);
                    break;
                case "10y":
                    mess.SendDate = currentDate.AddYears(10);
                    break;
                default:
                    mess.SendDate = currentDate;
                    break;
            }
            
            _checkService.AddMessage(mess);
            
            return RedirectToAction("Confirmation");
        }

        return View("Index");
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}


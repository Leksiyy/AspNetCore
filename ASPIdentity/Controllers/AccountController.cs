using Microsoft.AspNetCore.Mvc;

namespace ASPIdentity.Controllers;

public class AccountController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
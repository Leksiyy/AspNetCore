using Microsoft.AspNetCore.Mvc;

namespace homework10.Controllers;

public class CommentController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
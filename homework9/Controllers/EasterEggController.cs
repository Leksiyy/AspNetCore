using Microsoft.AspNetCore.Mvc;

namespace homework9.Controllers;

public class EasterEggController : Controller
{
    public IActionResult EasterEgg()
    {
        return Redirect("https://www.youtube.com/watch?v=jNQXAC9IVRw");
    }
}
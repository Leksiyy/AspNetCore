using Microsoft.AspNetCore.Mvc;

namespace homework12.Controllers;

public class SettingsController : Controller
{
    public IActionResult Index()
    {
        var theme = bool.Parse(HttpContext.Request.Cookies["theme"]);
        return View(theme);
    }

    [HttpPost]
    public IActionResult SetSettings(bool theme)
    {
        HttpContext.Response.Cookies.Append("theme", theme.ToString(), new CookieOptions { Expires = DateTimeOffset.Now.AddYears(1) });

        return RedirectToAction("Index","Home");
    }
}
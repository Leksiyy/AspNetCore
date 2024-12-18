using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ASPCookie.Models;

namespace ASPCookie.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (HttpContext.Session.Keys.Contains("data"))
        {
            return Content(HttpContext.Session.GetString("data"));
        }
        else
        {
            //CookieOptions cookieOptions = new CookieOptions()
            //{
            //    Expires = DateTimeOffset.Now.AddDays(1),
            //    SameSite = SameSiteMode.None,
            //    Secure = true,
            //    HttpOnly = true,
            //    // Path = "/"
            //};
            HttpContext.Session.SetString("data", "Some data..");
            return Content("Cookies is null");
        }
    }

    public IActionResult GetCookie()
    {
        string name = "Empty";
        if (HttpContext.Request.Cookies.ContainsKey("name"))
        {
            name = HttpContext.Request.Cookies["name"];
        }
        return Content(name);
    }

    public IActionResult SetCookie()
    {
        if (!HttpContext.Request.Cookies.ContainsKey("name"))
        {
            HttpContext.Response.Cookies.Append("name", "Tom Holand");
        }

        return Content("Cookie set!");
    }
}
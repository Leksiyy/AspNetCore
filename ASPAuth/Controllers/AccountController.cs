using System.Security.Claims;
using ASPAuth.Models;
using ASPAuth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ASPAuth.Controllers;

public class AccountController : Controller
{
    private readonly UserService _userService;

    public AccountController(UserService userService)
    {
        _userService = userService;
    }
    
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(User user)
    {
        User currentUser = _userService.GetUser(user.Email);
        if (currentUser == null)
        {
            return StatusCode(401);
        }
        
        var claims = new List<Claim> {new Claim(ClaimTypes.Name, user.Email)};
        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        
        return Redirect("/");
    }
}
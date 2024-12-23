using System.Security.Claims;
using homework13.Models;
using homework13.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace homework13.Controllers;

public class AuthController : Controller
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Registration(User user)
    {
        if (ModelState.IsValid)
        {
            if (_userService.IsUserNameTaken(user.UserName))
            {
                ViewBag.ErrorMessage = "Username already taken!";
                return View("Registration");
            }
            else
            {
                _userService.AddUser(user);
                
                // мне кажеться удобным автоматический SignIn после регистрации.
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName) };
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                
                return RedirectToAction("Index", "Home");
            }
        }
        ViewBag.ErrorMessage = "Model is not valid!";
        
        return View("Registration");
    }
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Login(User user)
    {
        if (ModelState.IsValid)
        {
            if (_userService.Login(user))
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName) };
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password!";
                
                return View("Login");
            }
        }
        ViewBag.ErrorMessage = "Model is not valid!";
        return View("Login");
    }
}
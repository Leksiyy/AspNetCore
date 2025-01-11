using Blog.Interfaces;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    [Route("login")]
    [HttpGet]
    public IActionResult Login(string returnUrl = "/")
    {
        return View(new LoginViewModel{ ReturnUrl = returnUrl });
    }

    [Route("login")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (result.Succeeded)
            {
                if (string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Route("register")]
    public async Task<IActionResult> Register([FromServices] IMembership membership, string? code)
    {
        if (!User.Identity.IsAuthenticated || code is not null)
        {
            if (await membership.ExistsMembershipByCodeAsync(code))
            {
                if (await membership.EnableCodeMembershipByCodeAsync(code))
                {
                    return View(new RegisterViewModel { Code = code });
                }
            }
        }
        return RedirectToAction("Index", "Home");
    }
    
    [Route("register")]
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Register([FromServices] IMembership membership, RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userCheck = await userManager.FindByEmailAsync(model.Email);
            if (userCheck != null)
            {
                ModelState.AddModelError("Email", "Такой email адрес уже есть!");
                return View(model);
            }
            User user = new User { Email = model.Email, UserName = model.Email, Name = model.Name, PhoneNumber = model.Phone };
            // добавляем пользователя
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //Установки роли. Сама роль находится в таблице AspNetRoles
                //если таблица пустая, получим ошибку. Обязательно заполняем роли!
                await userManager.AddToRoleAsync(user, "Editor");
                //используем приглашение
                await membership.DisableMembershipCodeAsync(model.Code);
                //установка куки
                await signInManager.SignInAsync(user, true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        return View(model);
    }
    
}
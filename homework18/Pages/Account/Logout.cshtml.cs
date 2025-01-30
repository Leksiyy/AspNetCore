using homework18.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace homework18.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly SignInManager<User> _signInManager;

    public LogoutModel(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Выход из системы
        await _signInManager.SignOutAsync();

        // Перенаправление на главную страницу или на returnUrl
        return RedirectToPage("/Index");
    }
}
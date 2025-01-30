using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using homework18.Models;

namespace homework18.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<User> _signInManager;

    public LoginModel(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    [BindProperty]
    public string Email { get; set; }
    [BindProperty]
    public string Password { get; set; }
    public string ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(Email, Password, isPersistent: true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                ErrorMessage = "Invalid login attempt.";
                return Page();
            }
        }
        return Page();
    }
}

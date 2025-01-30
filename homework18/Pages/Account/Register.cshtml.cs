using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using homework18.Models;

namespace homework18.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public RegisterModel(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [BindProperty]
    public string Email { get; set; }
    [BindProperty]
    public string Password { get; set; }
    public string ErrorMessage { get; set; }
    public string SuccessMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = new User { UserName = Email, Email = Email };
            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/Index");
            }
            else
            {
                ErrorMessage = "Error during registration: " + string.Join(", ", result.Errors);
                return Page();
            }
        }
        return Page();
    }
}
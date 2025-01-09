using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [Display(Name = "Email")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string? Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
    
    public string? ReturnUrl { get; set; }
}
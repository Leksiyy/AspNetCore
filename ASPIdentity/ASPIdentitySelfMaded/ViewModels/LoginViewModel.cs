using System.ComponentModel.DataAnnotations;

namespace ASPIdentitySelfMaded.ViewModels;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Email")]
    public string? Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string? Password { get; set; }
    
    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
    
    public string? ReturnUrl { get; set; }
}
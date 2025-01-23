using System.ComponentModel.DataAnnotations;

namespace homework16.ViewModels;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address.")]
    public string? Email { get; set; }
    

    [Required]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    
    [Required]
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords don't match.")]
    public string? ConfirmPassword { get; set; }
}
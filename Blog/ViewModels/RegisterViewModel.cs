using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Name is required")]
    [Display(Name = "Name")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Phone number is required")]
    [Display(Name = "Phone number")]
    public string? Phone { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [Display(Name = "Password")]
    [MinLength(5, ErrorMessage = "Password must be at least 5 characters long")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Confirm password is required")]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Passwords don't match")]
    [DataType(DataType.Password)]
    public string? PasswordConfirm { get; set; }
    
    public string? Code { get; set; }
}
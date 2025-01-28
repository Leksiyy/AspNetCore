using System.ComponentModel.DataAnnotations;

namespace homework17.ViewModels;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    
    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
    
    [Required]
    [Display(Name = "Group")]
    public string Group { get; set; }
    
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
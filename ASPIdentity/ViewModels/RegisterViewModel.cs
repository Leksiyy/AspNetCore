using System.ComponentModel.DataAnnotations;

namespace ASPIdentity.ViewModels;

public class RegisterViewModel
{
    [Required]
    public string? Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    
    [Required]
    [Compare("Password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    public string? ConfirmPassword { get; set; }
}
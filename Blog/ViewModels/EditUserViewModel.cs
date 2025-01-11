using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels;

public class EditUserViewModel
{
    [Key]
    public string? Id { get; set; }
    
    [Required(ErrorMessage = "Please enter your name")]
    [Display(Name = "Name")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Please enter your email")]
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Please enter your phone number")]
    [Display(Name = "Phone number")]
    public string? Phone { get; set; }
}
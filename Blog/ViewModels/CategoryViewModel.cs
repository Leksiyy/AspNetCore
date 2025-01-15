using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels;

public class CategoryViewModel
{
    [Key]
    public Guid Id { get; set; }
    
    [Display(Name = "Name")]
    [Required(ErrorMessage = "Not entered category name")]
    public string? Name { get; set; }
    
    [Display(Name = "Description")]
    public string? Description { get; set; }
}

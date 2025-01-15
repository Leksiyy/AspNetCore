using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.ViewModels;

public class PublicationViewModel
{
    [Key]
    public Guid? Id { get; set; } 
    
    [Display (Name = "зaгoлoвок")]
    [Required(ErrorMessage = "Не указан заголовок публикации")]
    public string? Title { get; set; }
    
    [Display(Name = "Содержимое")]
    [Required(ErrorMessage = "Не указано содержимое публикации")]
    public string? Description { get; set; }
    
    [Display (Name = "Kатегоpии")]
    public IEnumerable<SelectListItem>? SelectListCategories { get; set; }
    
    [Display (Name="Изображение")]
    public IFormFile? File { get; set; }
    
    public string? Image { get; set; }
    public string? ImageFullName { get; set; }
    
    [Display (Name = "Seo описание (до 155 символов)")]
    [Required(ErrorMessage = "He указано Sео описание")]
    [MaxLength(155, ErrorMessage = "укажите до 155 символов")]
    public string? SeoDescription { get; set; }
    
    [Display (Name = "Seо ключевые слова")]
    public string? Keywords { get; set; }
}
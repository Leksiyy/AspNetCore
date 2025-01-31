using System.ComponentModel.DataAnnotations;

namespace ASPWebApiSelfMaded.Models;

public class User
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Username is required")]
    public string Name { get; set; }
    
    [Range(1,100,ErrorMessage = "Please enter a number between 1 and 100")]
    [Required(ErrorMessage = "Age is required")]
    public int Age { get; set; }
}
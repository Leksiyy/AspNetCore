using System.ComponentModel.DataAnnotations;

namespace homework13.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Имя пользователя обязательно")]
    [StringLength(50, ErrorMessage = "Имя пользователя должно быть не более 50 символов")]
    public string UserName { get; set; }
    
    [Required(ErrorMessage = "Пароль обязателен")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 100 символов")]
    public string Password { get; set; }
}
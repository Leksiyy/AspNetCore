using System.ComponentModel.DataAnnotations;

namespace ASPValidation.Models;

public class Person
{
    [Key]
    public int Id { get; set; }
    [Display(Name = "Имя", Prompt = "Введите имя")]
    [Required(ErrorMessage = "Имя обязательно для заполнения!")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Укажите имя длинной от 2-ух до 100 символов")]
    public string Name { get; set; }
    [Display(Name = "Возраст")]
    [Required(ErrorMessage = "Возраст обязателен для заполнения!")]
    [Range(minimum: 1, maximum: 110, ErrorMessage = "Укажите возраст в промежутке от 1 до 110")]
    public int? Age { get; set; }
    [Display(Name = "Зарплата")]
    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Укажите заработную плату от 0")]
    [Required(ErrorMessage = "Зарплата обязательна для заполнения!")]
    public decimal? Salary { get; set; }
    
    public string Password { get; set; }
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string PasswordConfirm { get; set; }
    
    [Required(ErrorMessage = "Укажите ел адрес")]
    [EmailAddress(ErrorMessage = "Некорректный адрес")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Enter prone number!")]
    [Phone(ErrorMessage = "Incorrect phone number!")]
    public string Phone { get; set; }
    
    [Required(ErrorMessage = "Enter credit card number!")]
    [CreditCard(ErrorMessage = "Incorrect credit card number!")]
    public string BankCard { get; set; }
    
    
    [Required(ErrorMessage = "Enter Url!")]
    [Url(ErrorMessage = "Incorrect url!")]
    public string SiteAddress { get; set; }
}
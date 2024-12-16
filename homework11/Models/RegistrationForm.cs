using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace homework11.Models;

// 1) [ValidateNever]: Исключает поле "TermsOfService" из проверки. +
// 2) [CreditCard]: Проверяет формат номера кредитной карты в поле "CreditCardNumber". +
// 3) [Compare]: Сравнивает пароли в полях "Password" и "ConfirmPassword". +
// 4) [EmailAddress]: Проверяет формат адреса электронной почты в поле "Email". +
// 5) [PhoneNumber]: Проверяет формат номера телефона в поле "PhoneNumber". +
// 6) [Range]: Ограничивает возраст пользователя в поле "Age" диапазоном от 18 до 100 лет. +
// 7) [RegularExpression]: Проверяет, содержит ли имя пользователя в поле "Username" только буквы, цифры и подчеркивания. +
// 8) [Required]: Делает поля "FirstName", "LastName", "Password", "ConfirmPassword" и "Email" обязательными. +
// 9) [StringLength]: Ограничивает длину имени пользователя (поле "Username") 20 символами, а пароля (поля "Password" и "ConfirmPassword") - 100 символами. +
// 10) [URL]: Проверяет формат URL-адреса в поле "Website". +
// 11) [Remote]: Проверяет уникальность имени пользователя ("Username") +

public class RegistrationForm
{
    [Key]
    public int Id { get; set; }
    
    [Display(Name = "Имя", Prompt = "Введите имя")]
    [Required(ErrorMessage = "Имя обязательно для заполнения!")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Укажите имя длинной от 2-ух до 100 символов")]
    public string FirstName { get; set; }
    
    [Display(Name = "Имя", Prompt = "Введите фамилию")]
    [Required(ErrorMessage = "Фамилия обязательно для заполнения!")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Укажите имя длинной от 2-ух до 100 символов")]
    public string LastName { get; set; }
    
    [Display(Name = "Возраст")]
    [Required(ErrorMessage = "Возраст обязателен для заполнения!")]
    [Range(minimum: 18, maximum: 100, ErrorMessage = "Укажите возраст в промежутке от 18 до 100")]
    public int? Age { get; set; }
    
    [Remote(action: "ValidateUserName", controller: "Home")]
    [StringLength(20, ErrorMessage = "Длинна поля UserName должна быть не больше 20 символов")]
    [RegularExpression("/[a-zA-Z0-9\\-]+/g", ErrorMessage = ("UserName может содержать только буквы, цифры и подчеркивания."))]
    public string UserName { get; set; }
    
    [Display(Name = "Зарплата")]
    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Укажите заработную плату от 0")]
    [Required(ErrorMessage = "Зарплата обязательна для заполнения!")]
    public decimal? Salary { get; set; }

    [StringLength(100, ErrorMessage = "Длинна пароля должна быть не больше 100 символов")]
    [Required(ErrorMessage = "Пароль обязателен для заполнения!")]
    public string Password { get; set; }
    
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [StringLength(2100, ErrorMessage = "Длинна подтверждения пароля должна быть не больше 100 символов")]
    [Required(ErrorMessage = "Подтверждение пароля обязательно для заполнения!")]
    public string PasswordConfirm { get; set; }

    [Required(ErrorMessage = "Укажите ел адрес")]
    [EmailAddress(ErrorMessage = "Некорректный адрес")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Enter prone number!")]
    [Phone(ErrorMessage = "Incorrect phone number!")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Enter credit card number!")]
    [CreditCard(ErrorMessage = "Incorrect credit card number!")]
    public string CreditCardNumber { get; set; }
    
    [Required(ErrorMessage = "Enter Url!")]
    [Url(ErrorMessage = "Incorrect url!")]
    public string SiteAddress { get; set; }
    
    [ValidateNever]
    public bool TermsOfService { get; set; }
}
namespace ASPIdentitySelfMaded.ViewModels;

public class CreateUserViewModel
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int Year { get; set; }
}

public class EditUserViewModel
{
    public int? Id { get; set; }
    public string? Email { get; set; }
    public int Year { get; set; }
}
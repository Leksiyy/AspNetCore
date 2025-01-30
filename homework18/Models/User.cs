using Microsoft.AspNetCore.Identity;

namespace homework18.Models;

public class User : IdentityUser
{
    public List<ToDo>? ToDos { get; set; }
}
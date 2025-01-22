using Microsoft.AspNetCore.Identity;

namespace ASPIdentitySelfMaded.Models;

public class User : IdentityUser
{
    public int Year { get; set; }
}
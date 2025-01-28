using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace homework17.Models;

public class Student : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Group { get; set; }
    public ICollection<Grade> Grades { get; set; }
}

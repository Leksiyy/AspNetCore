using ASPAuth.Models;

namespace ASPAuth.Services;

public class UserService
{
    private List<User> _users;

    public UserService()
    {
        _users = new List<User>()
        {
            new User { Email = "admin@admin.com", Password = "admin123" },
            new User { Email = "alex@admin.com", Password = "admin123" },
        };
    }
    public User? GetUser(string email) => _users.FirstOrDefault(u => u.Email == email); 
}
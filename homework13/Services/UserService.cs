using ASPEntityFrameworkCore.Data;
using homework13.Models;
using homework13.Utilities;
using Microsoft.EntityFrameworkCore;

namespace homework13.Services;

public interface IUserService
{
    bool IsUserNameTaken(string userName);
    void AddUser(User user);
    bool Login(User user);
}

public class UserService : IUserService
{
    private readonly ApplicationContext _context;

    public UserService(ApplicationContext context)
    {
        _context = context;
    }

    public bool IsUserNameTaken(string userName)
    {
        return _context.Users.Any(u => u.UserName == userName);
    }

    public void AddUser(User user)
    {
        user.EncryptPassword();
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public bool Login(User user)
    {
        user.EncryptPassword();
        
        User? existingUser = _context.Users
            .FirstOrDefault(u => u.UserName == user.UserName);

            var qwer = existingUser != null && user.ValidatePassword(existingUser.Password);
        return existingUser != null && user.ValidatePassword(existingUser.Password);
    }
}
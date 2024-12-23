using homework13.Models;

namespace homework13.Utilities;

using System;
using System.Security.Cryptography;
using System.Text;

public static class PasswordTweaker
{
    private const string Salt = "QwErTy321";

    public static void EncryptPassword(this User user)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = Salt + user.Password;

            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

            user.Password = Convert.ToHexString(hashBytes);
        }
    }

    public static bool ValidatePassword(this User user, string storedHash)
    {
        return user.Password == storedHash;
    }
}
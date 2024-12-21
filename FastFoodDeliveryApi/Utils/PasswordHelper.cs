using System.Security.Cryptography;
using System.Text;

namespace FastFoodDeliveryApi.Util;

public static class PasswordHelper
{
    public static string HashPassword(string password)
    {
        // using var sha256 = SHA256.Create();
        // var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        // return Convert.ToBase64String(hashBytes);
        return password;
    }
    
    public static bool VerifyPassword(string rawPassword, string storedHashedPassword)
    {
        // var hashedRawPassword = HashPassword(rawPassword);
        // return hashedRawPassword == storedHashedPassword;
        return rawPassword == storedHashedPassword;
    }
}
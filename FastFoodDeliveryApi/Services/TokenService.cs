using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FastFoodDeliveryApi.Models.Models.Account;
using FastFoodDeliveryApi.Util;
using FastFoodDeliveryApi.Models.Models;

namespace FastFoodDeliveryApi.Services;

public class TokenService
{
    public const string BearerToken = "Bearer ";
    public const string RegisterToken = "Register ";
    public const string ResetPasswordToken = "ResetPassword ";
    
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private string GenerateJwtToken(IEnumerable<Claim> claims, string prefixType)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException()));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1), // Token expiration set to 1 hour
            signingCredentials: credentials);

        return prefixType + new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Method to generate token for authentication (Login)
    public string GenerateToken(string username)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username)
        };

        return GenerateJwtToken(claims, BearerToken);
    }

    // Method to generate token for registration (e.g., includes verification code)
    public string GenerateRegisterToken(RegisterModel model, string verificationCode)
    {
        var hashedPassword = PasswordHelper.HashPassword(model.Password);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, model.FirstName),
            new Claim(ClaimTypes.Email, model.Email),
            new Claim("HashedPassword", hashedPassword),
            new Claim("VerificationCode", verificationCode)  // Add verification code as a claim
        };

        return GenerateJwtToken(claims, RegisterToken);
    }
        
    public Dictionary<string, string> ParseRegisterToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        token = token.Replace(RegisterToken, "");
        try
        {
            var claims = handler.ReadJwtToken(token).Claims;
            var registrationData = new Dictionary<string, string>
            {
                { "FirstName", claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value },
                { "Email", claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value },
                { "Password", claims.FirstOrDefault(c => c.Type == "HashedPassword")?.Value },
                { "VerificationCode", claims.FirstOrDefault(c => c.Type == "VerificationCode")?.Value }
            };

            return registrationData;
        }
        catch (Exception ex)
        {
            return new Dictionary<string, string>
            {
                {"ErrorMessage", $"Invalid Token: {ex.Message}"}
            };
        }
    }
    
    public string GenerateResetPasswordToken(string email, string verificationCode)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim("VerificationCode", verificationCode)  // Add verification code as a claim
        };

        return GenerateJwtToken(claims, ResetPasswordToken);
    }


    public Dictionary<string, string> ParseResetPasswordToken(String token)
    {
        var handler = new JwtSecurityTokenHandler();
        token = token.Replace(ResetPasswordToken, "");
        try
        {
            var claims = handler.ReadJwtToken(token).Claims;
            var resetPasswordData = new Dictionary<string, string>
            {
                { "Email", claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value },
                { "VerificationCode", claims.FirstOrDefault(c => c.Type == "VerificationCode")?.Value }
            };

            return resetPasswordData;
        }
        catch (Exception ex)
        {
            return new Dictionary<string, string>
            {
                {"ErrorMessage", $"Invalid Token: {ex.Message}"}
            };
        }
    }
}
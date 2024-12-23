using FastFoodDeliveryApi.Data;
using FastFoodDeliveryApi.Models.Entities;
using FastFoodDeliveryApi.Models.Models.Account;
using FastFoodDeliveryApi.Services;
using FastFoodDeliveryApi.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FastFoodDeliveryApi.Models.Models;

namespace FastFoodDeliveryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;
    private readonly EmailService _emailService;

    public AccountController(AppDbContext context, TokenService tokenService, EmailService emailService)
    {
        _context = context;
        _tokenService = tokenService;
        _emailService = emailService;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password) ||
            string.IsNullOrWhiteSpace(model.FirstName))
        {
            return BadRequest("Invalid input. All fields are required.");
        }

        if (await _context.Users.AnyAsync(x => x.Email == model.Email))
        {
            return BadRequest("Email is already taken.");
        }
        
        var verificationCode = new Random().Next(1000, 9999).ToString();

        try
        {
            await _emailService.SendEmailAsync(model.Email, "Verify Your Email", $"Your verification code is: {verificationCode}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error sending verification email.");
        }

        var token = _tokenService.GenerateRegisterToken(model, verificationCode);
        
        return Ok(new { Message = "Verification code sent successfully. Verify Your Email", RegistrationToken = token });
    }
    
    [HttpPost]
    [Route("Verify-Email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }

        if (model.VerificationToken.StartsWith(TokenService.RegisterToken))
        {
            var registrationData = _tokenService.ParseRegisterToken(model.VerificationToken);

            if (registrationData.ContainsKey("ErrorMessage"))
            {
                return BadRequest(registrationData.FirstOrDefault(kvp => kvp.Key == "ErrorMessage").Value);
            }
        
            var firstName = registrationData["FirstName"];
            var email = registrationData["Email"];
            var password = registrationData["Password"];
            var verificationCode = registrationData["VerificationCode"];

            var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Customer");

            if (customerRole == null)
            {
                return BadRequest("Customer role not found.");
            }
            
            var customerRoleList = new List<Role> { customerRole };
            
            var user = new User
            {
                FirstName = firstName,
                Email = email,
                Password = password,
                Roles = customerRoleList
            };
        
            if(model.VerificationCode != verificationCode)
                return BadRequest("Invalid Verification Code.");

            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return BadRequest("Email already registered.");
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok("Email registered successfully.");
        }

        if (model.VerificationToken.StartsWith(TokenService.ResetPasswordToken))
        {
            var tokenData = _tokenService.ParseResetPasswordToken(model.VerificationToken);
            
            if (tokenData.ContainsKey("ErrorMessage"))
            {
                return BadRequest(tokenData.FirstOrDefault(kvp => kvp.Key == "ErrorMessage").Value);
            }
            
            var email = tokenData["Email"];
            var verificationCode = tokenData["VerificationCode"];
            
            if(model.VerificationCode != verificationCode)
                return BadRequest("Invalid Verification Code.");

            return Ok(new { message = "Email verified successfully. Enter new password." });

        }
        return BadRequest("Something went wrong.");
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!await _context.Users.AnyAsync()) 
            return NotFound();
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(model.Email));
        if (user == null)
            return NotFound();
        
        if (!PasswordHelper.VerifyPassword(model.Password, user.Password)) 
            return BadRequest("Invalid password.");

        var token = _tokenService.GenerateToken(user.Email);
        
        return Ok(new {Token = token});
    }

    [HttpPost]
    [Route("Reset-Password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        if (!await _context.Users.AnyAsync(u => u.Email.Equals(model.Email)))
        {
            return BadRequest("Invalid email.");
        }
        
        var verificationCode = new Random().Next(1000, 9999).ToString();
        
        try
        {
            await _emailService.SendEmailAsync(model.Email, "Verify Your Email", $"Your verification code is: {verificationCode}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error sending verification email.");
        }
        
        var resetPasswordToken = _tokenService.GenerateResetPasswordToken(model.Email, verificationCode);
        return Ok(new { Message = "Verification code sent successfully. Verify Your Email", ResetPasswordToken = resetPasswordToken });
    }

    [HttpPost]
    [Route("Enter-New-Password")]
    public async Task<IActionResult> EnterNewPassword([FromBody] NewPasswordModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var tokenData = _tokenService.ParseResetPasswordToken(model.VerificationToken);
        if (tokenData.TryGetValue("ErrorMessage", out var value))
        {
            return BadRequest(value);
        }

        var email = tokenData["Email"];
        var verificationCode = tokenData["VerificationCode"];

        if (model.VerificationCode != verificationCode)
        {
            return Unauthorized("Invalid verification code.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (PasswordHelper.VerifyPassword(model.NewPassword, user.Password))
        {
            return Conflict("The new password cannot be the same as the old password.");
        }

        user.Password = PasswordHelper.HashPassword(model.NewPassword);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Password changed successfully.", email });
    }
    
}
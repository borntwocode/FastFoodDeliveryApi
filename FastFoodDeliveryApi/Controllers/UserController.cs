using FastFoodDeliveryApi.Data;
using FastFoodDeliveryApi.Models.Entities;
using FastFoodDeliveryApi.Models.Models.User;
using FastFoodDeliveryApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastFoodDeliveryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        if (!await _context.Users.AnyAsync())
        {
            return NotFound();
        }
        var users = await _context.Users
            .Include(user => user.UserAddress)
            .ToListAsync();
        return Ok(users);
    }

    [HttpPut]
    [Route("EditUser")]
    public async Task<IActionResult> EditUser([FromBody] EditUserModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = model.UserId;

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        user.FirstName = model.FirstName;
        user.PhoneNumber = model.PhoneNumber;

        await _context.SaveChangesAsync();
        return Ok(new { message = "User updated successfully." });
    }
    
    [HttpPut]
    [Route("EditUserAddress")]
    public async Task<IActionResult> EditUserAddress([FromBody] EditUserAddressModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _context.Users
            .Include(u => u.UserAddress)
            .FirstOrDefaultAsync(u => u.Id == model.UserId);

        if (user == null)
        {
            return NotFound(new { Message = "User not found." });
        }

        var baseAddress = await LocationIdentifier.GetLocation(model.Latitude, model.Longitude);

        if (baseAddress != null)
        {
            if (user.UserAddress == null)
            {
                var userAddress = new UserAddress
                {
                    City = baseAddress.City,
                    County = baseAddress.County,
                    Residential = baseAddress.Residential,
                    Road = baseAddress.Road,
                    User = user
                };
                _context.UserAddresses.Add(userAddress);
            }
            else
            {
                user.UserAddress.City = baseAddress.City;
                user.UserAddress.County = baseAddress.County;
                user.UserAddress.Residential = baseAddress.Residential;
                user.UserAddress.Road = baseAddress.Road;
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = "User address updated successfully.", Address = baseAddress });
        }
        return BadRequest(new { Message = "Invalid coordinates. Try again." });
    }
    
}
using FastFoodDeliveryApi.Data;
using FastFoodDeliveryApi.Models.Entities;
using FastFoodDeliveryApi.Models.Models.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastFoodDeliveryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    
    private readonly AppDbContext _context;

    public RoleController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _context.Roles.ToListAsync();
        if (roles == null)
        {
            return NotFound();
        }
        return Ok(roles);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddRole([FromBody] RoleModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var role = new Role {Name = model.Name};
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
        return Ok(role);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound($"Role with ID {id} not found.");
        }
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "Role deleted successfully." });
    }
    
}
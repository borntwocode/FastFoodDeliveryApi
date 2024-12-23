using FastFoodDeliveryApi.Models.Entities;

namespace FastFoodDeliveryApi.Data.Seeder;

public class DatabaseSeeder
{
    private readonly AppDbContext _context;

    public DatabaseSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedRolesAsync()
    {
        if (!_context.Roles.Any())
        {
            _context.Roles.AddRange(
                new Role { Name = "Admin" },
                new Role { Name = "Customer" },
                new Role { Name = "Delivery" }
            );

            await _context.SaveChangesAsync();
        }
    }
}
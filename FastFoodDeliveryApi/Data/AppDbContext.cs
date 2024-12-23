using FastFoodDeliveryApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastFoodDeliveryApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    public DbSet<User> Users { get; set; }
    public DbSet<UserAddress> UserAddresses { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<RestaurantAddress> RestaurantAddresses { get; set; }
    public DbSet<Models.Entities.File> Files { get; set; }
    
}
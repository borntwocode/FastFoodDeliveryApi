using FastFoodDeliveryApi.Data;
using FastFoodDeliveryApi.Models.Entities;
using FastFoodDeliveryApi.Models.Models.Restaurant;
using FastFoodDeliveryApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastFoodDeliveryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantController : ControllerBase
{
    
    private readonly AppDbContext _context;

    public RestaurantController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("GetRestaurants")]
    public async Task<IActionResult> GetRestaurants()
    {
        var restaurants = await _context.Restaurants
            .Include(restaurant => restaurant.RestaurantAddress)
            .ToListAsync();
        if (restaurants == null)
        {
            return NotFound();
        }
        return Ok(restaurants);
    }

    [HttpPost]
    [Route("AddRestaurant")]
    public async Task<IActionResult> AddRestaurant([FromBody] RestaurantModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var restaurant = new Restaurant
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Description = model.Description,
            PhoneNumber = model.PhoneNumber,
            Website = model.Website
        };
        
        await _context.Restaurants.AddAsync(restaurant);
        await _context.SaveChangesAsync();

        if (model.Latitude != 0 && model.Longitude != 0)
        {
            var location = LocationIdentifier.GetLocation(model.Latitude, model.Longitude);
            var restaurantAddress = new RestaurantAddress
            {
                Id = Guid.NewGuid(),
                City = location.Result.City,
                County = location.Result.County,
                Residential = location.Result.Residential,
                Road = location.Result.Road,
                Restaurant = restaurant
            };
            await _context.RestaurantAddresses.AddAsync(restaurantAddress);
            await _context.SaveChangesAsync();
        }
        
        return Ok(new { Message = "Restaurant added successfully.", Restaurant = restaurant });
    }

    [HttpPut]
    [Route("SetPhotoId")]
    public async Task<IActionResult> SetPhotoId([FromBody] RestaurantPhotoModel model)
    {
        var restaurant = await _context.Restaurants.FindAsync(model.RestaurantId);
        if (restaurant == null)
        {
            return NotFound(new { Message = "Restaurant not found." });
        }

        if (!await _context.Files.AnyAsync(file => file.FileId == model.PhotoId))
        {
            return BadRequest(new { Message = "Photo not found." });
        }

        restaurant.ProfilePictureId = model.PhotoId;
        await _context.SaveChangesAsync();
        return Ok(new { Message = "Restaurant photo updated successfully." });
    }
    
    [HttpPut]
    [Route("EditAddress")]
    public async Task<IActionResult> EditAddress([FromBody] RestaurantAddressModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var restaurant = await _context.Restaurants
            .Include(rest => rest.RestaurantAddress)
            .FirstOrDefaultAsync(rest => rest.Id == model.RestaurantId);
        if (restaurant == null)
        {
            return NotFound(new { Message = "Restaurant not found." });
        }

        var location = LocationIdentifier.GetLocation(model.Latitude, model.Longitude);
        if (restaurant.RestaurantAddress == null)
        {
            var restaurantAddress = new RestaurantAddress
            {
                Id = Guid.NewGuid(),
                City = location.Result.City,
                County = location.Result.County,
                Residential = location.Result.Residential,
                Road = location.Result.Road,
                Restaurant = restaurant
            };
            await _context.RestaurantAddresses.AddAsync(restaurantAddress);
        }
        else
        {
            restaurant.RestaurantAddress.City = location.Result.City;
            restaurant.RestaurantAddress.County = location.Result.County;
            restaurant.RestaurantAddress.Residential = location.Result.Residential;
            restaurant.RestaurantAddress.Road = location.Result.Road;
        }
        await _context.SaveChangesAsync();
        
        return Ok(new { Message = "Restaurant address updated successfully.", Address = location.Result });
    }
    
}
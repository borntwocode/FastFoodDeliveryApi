using System.ComponentModel.DataAnnotations;

namespace FastFoodDeliveryApi.Models.Models.Restaurant;

public class RestaurantModel
{
    [Required( ErrorMessage = "Restaurant name is required.")]
    public string Name { get; set; }
    
    [Required( ErrorMessage = "Restaurant description is required.")]
    public string Description { get; set; }
    
    [Required( ErrorMessage = "Restaurant phone number is required.")]
    public string PhoneNumber { get; set; }
    
    public string? Website { get; set; }
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace FastFoodDeliveryApi.Models.Models.Restaurant;

public class RestaurantAddressModel
{
    [Required( ErrorMessage = "Restaurant id is required.")]
    public Guid RestaurantId { get; set; }
    
    [Required( ErrorMessage = "Latitude is required.")]
    public double Latitude { get; set; }
    
    [Required( ErrorMessage = "Longitude is required.")]
    public double Longitude { get; set; }
}
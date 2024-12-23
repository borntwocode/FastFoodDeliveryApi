using System.ComponentModel.DataAnnotations;

namespace FastFoodDeliveryApi.Models.Models.Restaurant;

public class RestaurantPhotoModel
{
    [Required( ErrorMessage = "Restaurant id is required.")]
    public Guid RestaurantId { get; set; }
    
    [Required( ErrorMessage = "Photo id is required.")]
    public Guid PhotoId { get; set; }
    
}
using System.ComponentModel.DataAnnotations;

namespace FastFoodDeliveryApi.Models.Entities;

public class Restaurant
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public RestaurantAddress? RestaurantAddress { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string? Website { get; set; }
    
    public Guid? ProfilePictureId { get; set; }
    
}
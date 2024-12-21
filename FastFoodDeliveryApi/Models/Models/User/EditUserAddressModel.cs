using System.ComponentModel.DataAnnotations;

namespace FastFoodDeliveryApi.Models.Models.User;

public class EditUserAddressModel
{
    [Required(ErrorMessage = "User id is required.")]
    public Guid UserId { get; set; }
    
    [Required(ErrorMessage = "Latitude is required.")]
    public double Latitude { get; set; }

    [Required(ErrorMessage = "Longitude is required.")]
    public double Longitude { get; set; }
}
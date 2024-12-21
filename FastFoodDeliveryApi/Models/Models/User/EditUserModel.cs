using System.ComponentModel.DataAnnotations;

namespace FastFoodDeliveryApi.Models.Models.User;

public class EditUserModel
{
    [Required(ErrorMessage = "User id is required.")]
    public Guid UserId { get; set; }
    
    [Required(ErrorMessage = "First Name is required")]
    [StringLength(50, ErrorMessage = "First Name must be between 2 and 50 characters", MinimumLength = 2)]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "Phone Number is required")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    [StringLength(15, ErrorMessage = "Phone Number cannot exceed 15 characters.")]
    public string PhoneNumber { get; set; }
}
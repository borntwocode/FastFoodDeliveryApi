using System.ComponentModel.DataAnnotations;

namespace FastFoodDeliveryApi.Models.Models.Account;

public record ResetPasswordModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; init; }
}
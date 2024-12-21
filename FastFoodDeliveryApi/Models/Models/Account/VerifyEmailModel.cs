using System.ComponentModel.DataAnnotations;

namespace FastFoodDeliveryApi.Models.Models.Account;

public class VerifyEmailModel
{
    [Required(ErrorMessage = "Verification code is required.")]
    [StringLength(4, MinimumLength = 4, ErrorMessage = "Verification code must be 4 digits.")]
    public string VerificationCode { get; init; }

    [Required(ErrorMessage = "Verification token is required.")]
    public string VerificationToken { get; init; }
}
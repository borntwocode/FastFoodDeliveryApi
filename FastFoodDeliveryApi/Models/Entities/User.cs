using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFoodDeliveryApi.Models.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? PhoneNumber { get; set; }
    public UserAddress UserAddress { get; set; }
    public Guid? ProfilePictureId { get; set; }
    [InverseProperty("Users")]
    public List<Role> Roles { get; set; }
}
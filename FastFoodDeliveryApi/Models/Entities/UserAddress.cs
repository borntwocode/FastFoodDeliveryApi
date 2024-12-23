using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using FastFoodDeliveryApi.Models.Entities.Abs;

namespace FastFoodDeliveryApi.Models.Entities;

public class UserAddress : BaseAddress
{
    [Key]
    public Guid UserId { get; set; }
    [JsonIgnore]
    [ForeignKey("UserId")]
    public User User { get; set; }

}
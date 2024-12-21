using System.Text.Json.Serialization;
using FastFoodDeliveryApi.Models.Entities.Abs;

namespace FastFoodDeliveryApi.Models.Entities;

public class UserAddress : BaseAddress
{
    public Guid UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }

}
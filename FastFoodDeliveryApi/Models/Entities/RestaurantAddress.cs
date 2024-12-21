using System.Text.Json.Serialization;
using FastFoodDeliveryApi.Models.Entities.Abs;

namespace FastFoodDeliveryApi.Models.Entities;

public class RestaurantAddress : BaseAddress
{
    public Guid RestaurantId { get; set; }
    [JsonIgnore]
    public Restaurant Restaurant { get; set; }
}
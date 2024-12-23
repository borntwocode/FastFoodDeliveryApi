using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using FastFoodDeliveryApi.Models.Entities.Abs;

namespace FastFoodDeliveryApi.Models.Entities;

public class RestaurantAddress : BaseAddress
{
    [Key]
    public Guid RestaurantId { get; set; }
    [JsonIgnore]
    [ForeignKey("RestaurantId")]
    public Restaurant Restaurant { get; set; }
}
namespace FastFoodDeliveryApi.Models.Entities.Abs;

public class BaseAddress
{
    public Guid Id { get; set; }
    public string? City { get; set; }
    public string? County { get; set; }
    public string? Residential { get; set; }
    public string? Road { get; set; }
}
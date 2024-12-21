namespace FastFoodDeliveryApi.Models.Entities;

public class Restaurant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public UserAddress UserAddress { get; set; }
    public string PhoneNumber { get; set; }
    public bool isOpen { get; set; }
    public string website { get; set; }
    public float Rating { get; set; }
    
}
namespace NotifyMe.Infrastructure.Models;

public class ProductInformation
{
    public bool IsDiscounted { get; set; }
    public string? DiscountedPrice { get; set; }
    public string Price { get; set; } = null!;
    public string Name { get; set; } = null!;
}
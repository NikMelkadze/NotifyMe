namespace NotifyMe.Infrastructure.Models;

public class ProductPriceInformation
{
    public bool IsDiscounted { get; set; }
    public string? DiscountedPrice { get; set; }
    public string Price { get; set; } = null!;
}
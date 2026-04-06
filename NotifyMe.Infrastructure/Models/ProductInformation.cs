namespace NotifyMe.Infrastructure.Models;

public class ProductInformation
{
    public bool IsDiscounted { get; set; }
    public decimal? DiscountedPrice { get; set; }
    public decimal Price { get; set; } 
    public string Name { get; set; } = null!;
}
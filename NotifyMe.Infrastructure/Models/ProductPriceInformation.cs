namespace NotifyMe.Infrastructure.Models;

public class ProductPriceInformation
{
     public bool IsDiscounted { get; set; }
     public string CurrentPrice { get; set; } = null!;
     public string? OldPrice { get; set; }
}
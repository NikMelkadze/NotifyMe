using NotifyMe.Infrastructure.Models;

namespace NotifyMe.Infrastructure.Contracts;

public interface IShopProductService<in TRequest>
{
    public string Price { get; set; } 
    public string? DiscountedPrice { get; set; }
    
    ProductPriceInformation GetPriceInformation(TRequest content);
    string GetProductName(TRequest content);
}
using NotifyMe.Infrastructure.Models;

namespace NotifyMe.Infrastructure.Contracts;

public interface IShopProductService<in TRequest>
{
    ProductPriceInformation GetPriceInformation(TRequest content);
    string GetProductName(TRequest content);
}
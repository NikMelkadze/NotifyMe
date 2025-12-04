using NotifyMe.Domain.Enums;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Models.ApiResponse;

namespace NotifyMe.Infrastructure.Services;

public class FetchDataFromJson : FetchDataFactory<ProductBase>
{
    public override Task<(bool isDiscounted, string currentPrice, string prevPrice)> GetDiscountInformation(ProductBase product, Shop shop, CancellationToken cancellationToken)
    {
        return Task.FromResult((product.Product.PreviousPrice != null, product.Product.Price.ToString(),
            product.Product.PreviousPrice.ToString()));
    }

    public override Task<string> GetProductName(ProductBase product, Shop shop, CancellationToken cancellationToken)
    {
        return Task.FromResult(product.Product.Name);
    }
}
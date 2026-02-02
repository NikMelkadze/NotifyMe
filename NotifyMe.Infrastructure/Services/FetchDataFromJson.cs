using System.Globalization;
using NotifyMe.Domain.Enums;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Models;
using NotifyMe.Infrastructure.Models.ApiResponse;

namespace NotifyMe.Infrastructure.Services;

public class FetchDataFromJson : FetchDataFactory<ProductBase>
{
    public override Task<ProductPriceInformation> GetDiscountInformation(ProductBase product, Shop shop,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new ProductPriceInformation()
        {
            IsDiscounted = product.Product.PreviousPrice != null,
            CurrentPrice = product.Product.Price.ToString(CultureInfo.InvariantCulture),
            OldPrice = product.Product.PreviousPrice.ToString()
        });
    }

    public override Task<string> GetProductName(ProductBase product, Shop shop, CancellationToken cancellationToken)
    {
        return Task.FromResult(product.Product.Name);
    }
}
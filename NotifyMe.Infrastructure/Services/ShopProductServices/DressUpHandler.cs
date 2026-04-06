using AngleSharp;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Infrastructure.Models;
using NotifyMe.Infrastructure.Services.Common;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class DressUpHandler(IHttpClientService httpClientService, IBrowsingContext browsingContext) : ShopHandlerBase(httpClientService, browsingContext)
{
    public override async Task<ProductInformation> GetProductInformation(string url, CancellationToken cancellationToken)
    {
        var document = await GetDocument(url, cancellationToken);
        var pricesRoot = document.QuerySelector(".product-prices");

        var regularPriceWhenDiscount = pricesRoot?
            .QuerySelector(".product-discount .regular-price")
            ?.TextContent;

        var discountedPriceWhenDiscount = pricesRoot?
            .QuerySelector(".current-price .product-price[itemprop='price']")
            ?.GetAttribute("content");

        if (regularPriceWhenDiscount != null)
        {
            DiscountedPrice = Convert.ToDecimal(discountedPriceWhenDiscount!.NormalizePrice());
            Price = Convert.ToDecimal(regularPriceWhenDiscount.NormalizePrice());
        }
        else
        {
            Price = Convert.ToDecimal(discountedPriceWhenDiscount!.NormalizePrice());
        }


        return new ProductInformation
        {
            IsDiscounted = DiscountedPrice != null,
            DiscountedPrice = DiscountedPrice,
            Price = Price,
            Name = GetProductName(document)
        };
    }
}
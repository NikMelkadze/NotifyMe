using AngleSharp;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Infrastructure.Models;
using NotifyMe.Infrastructure.Services.Common;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class EuroProductHandler(IHttpClientService httpClientService, IBrowsingContext browsingContext) : ShopHandlerBase(httpClientService, browsingContext)
{
    public override async Task<ProductInformation> GetProductInformation(string url, CancellationToken cancellationToken)
    {
        var document = await GetDocument(url, cancellationToken);
        var productItem = document.QuerySelector("div.product-item")!;

        var discountedPriceElWhenDiscount = productItem.QuerySelector("span.product-price span.new");
        var regularPriceElWhenDiscount = productItem.QuerySelector("span.product-price span.old");

        if (discountedPriceElWhenDiscount != null && regularPriceElWhenDiscount != null)
        {
            DiscountedPrice = discountedPriceElWhenDiscount.TextContent;
            Price = regularPriceElWhenDiscount.TextContent;
        }
        else
        {
            Price = document
                .QuerySelector("span.product-price > span")
                ?.TextContent!;
        }

        return new ProductInformation
        {
            DiscountedPrice = DiscountedPrice?.NormalizePrice(),
            Price = Price.NormalizePrice(),
            IsDiscounted = DiscountedPrice != null,
            Name = GetProductName(document)
        };
    }
}
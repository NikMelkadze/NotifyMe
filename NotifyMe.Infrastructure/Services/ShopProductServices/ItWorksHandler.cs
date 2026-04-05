using AngleSharp;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Infrastructure.Models;
using NotifyMe.Infrastructure.Services.Common;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class ItWorksHandler(IHttpClientService httpClientService, IBrowsingContext browsingContext) : ShopHandlerBase(httpClientService, browsingContext)
{
    public override async Task<ProductInformation> GetProductInformation(string url, CancellationToken cancellationToken)
    {
        var document = await GetDocument(url, cancellationToken);
        var pricesDiv = document.QuerySelector("div.prices-container");

        var regularPriceWhenDiscount = pricesDiv?.QuerySelector("#old-price")?.TextContent;

        if (regularPriceWhenDiscount != null)
        {
            Price = regularPriceWhenDiscount;
            DiscountedPrice = pricesDiv?.QuerySelector("#current-price")?.TextContent;
        }
        else
        {
            Price = pricesDiv?.QuerySelector("#current-price")?.TextContent!;
        }

        return new ProductInformation
        {
            IsDiscounted = DiscountedPrice != null,
            DiscountedPrice = DiscountedPrice?.NormalizePrice(),
            Price = Price.NormalizePrice(),
            Name = GetProductName(document)
        };
    }
}
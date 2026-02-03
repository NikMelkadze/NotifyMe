using AngleSharp.Dom;
using NotifyMe.Domain.Exceptions;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Models;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class ItWorksShopProductService : IShopProductService<IDocument>
{
    public ProductPriceInformation GetPriceInformation(IDocument content)
    {
        var pricesDiv = content.QuerySelector("div.prices-container");
        var currentPrice = pricesDiv?.QuerySelector("#current-price")?.TextContent?.Normalize() ?? "";
        var oldPrice = pricesDiv?.QuerySelector("#old-price")?.TextContent?.Normalize() ?? "";

        var isDiscounted = oldPrice != "";

        return new ProductPriceInformation
        {
            IsDiscounted = isDiscounted,
            CurrentPrice = currentPrice,
            OldPrice = oldPrice
        };
    }

    public string GetProductName(IDocument content)
    {
        var element = content.QuerySelector("meta[property='og:title']");
        return element?.GetAttribute("content") ?? throw new ValidationException("Wrong Domain");
    }
}
using AngleSharp.Dom;
using NotifyMe.Domain.Exceptions;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Infrastructure.Models;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class DressUpShopProductService : IShopProductService<IDocument>
{
    public ProductPriceInformation GetPriceInformation(IDocument content)
    {
        var pricesRoot = content.QuerySelector(".product-prices");

        var currentPrice = pricesRoot?
            .QuerySelector(".current-price .product-price[itemprop='price']")
            ?.GetAttribute("content")
            ?.NormalizePrice() ?? "";

        var oldPrice = pricesRoot?
            .QuerySelector(".product-discount .regular-price")
            ?.TextContent.NormalizePrice();

        var isDiscounted = !string.IsNullOrWhiteSpace(oldPrice);

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
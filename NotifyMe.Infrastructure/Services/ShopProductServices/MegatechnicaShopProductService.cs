using AngleSharp.Dom;
using NotifyMe.Domain.Exceptions;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Models;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class MegatechnicaShopProductService : IShopProductService<IDocument>
{
    public ProductPriceInformation GetPriceInformation(IDocument content)
    {
        var pricesDivMega = content.QuerySelector("div.prices");
        var oldPrice = pricesDivMega!.QuerySelector("span.prev_price")?.TextContent.Trim() ?? "";
        var oldPriceTrimmed = System.Text.RegularExpressions.Regex.Replace(oldPrice, @"[^\d]", "");
        var currentPrice = pricesDivMega!.QuerySelector("span.price")?.TextContent.Trim() ?? "";
        var isDiscounted = oldPrice != "";

        return new ProductPriceInformation
        {
            IsDiscounted = isDiscounted,
            CurrentPrice = currentPrice,
            OldPrice = oldPriceTrimmed
        };
    }

    public string GetProductName(IDocument content)
    {
        var element = content.QuerySelector("meta[property='og:title']");
        return element?.GetAttribute("content") ?? throw new ValidationException("Wrong Domain");
    }
}
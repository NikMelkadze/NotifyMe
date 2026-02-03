using AngleSharp.Dom;
using NotifyMe.Domain.Exceptions;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Infrastructure.Models;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class MegatechnicaShopProductService : IShopProductService<IDocument>
{
    public ProductPriceInformation GetPriceInformation(IDocument content)
    {
        var pricesDivMega = content.QuerySelector("div.prices");
        var oldPrice = pricesDivMega!.QuerySelector("span.prev_price")?.TextContent.NormalizePrice() ?? "";
        var currentPrice = pricesDivMega!.QuerySelector("span.price")?.TextContent.NormalizePrice() ?? "";
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
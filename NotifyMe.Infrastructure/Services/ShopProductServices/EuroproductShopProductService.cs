using AngleSharp.Dom;
using NotifyMe.Domain.Exceptions;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Infrastructure.Models;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class EuroproductShopProductService : IShopProductService<IDocument>
{
    public ProductPriceInformation GetPriceInformation(IDocument content)
    {
        var productItem = content.QuerySelector("div.product-item")!;

        var newPriceEl = productItem.QuerySelector("span.product-price span.new");
        var oldPriceEl = productItem.QuerySelector("span.product-price span.old");

        if (newPriceEl != null && oldPriceEl != null)
        {
            var currentPrice = newPriceEl.TextContent.NormalizePrice();
            var oldPrice = oldPriceEl.TextContent.NormalizePrice();

            return new ProductPriceInformation()
            {
                CurrentPrice = currentPrice,
                OldPrice = oldPrice,
                IsDiscounted = true
            };
        }
        else
        {
            var currentPrice = content
                .QuerySelector("span.product-price > span")
                ?.TextContent
                .NormalizePrice()!;

            return new ProductPriceInformation()
            {
                CurrentPrice = currentPrice,
                OldPrice = null,
                IsDiscounted = false
            };
        }
    }

    public string GetProductName(IDocument content)
    {
        var element = content.QuerySelector("meta[property='og:title']");
        return element?.GetAttribute("content") ?? throw new ValidationException("Wrong Domain");
    }
}
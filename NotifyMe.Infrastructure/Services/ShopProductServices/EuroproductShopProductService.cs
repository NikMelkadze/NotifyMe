using AngleSharp.Dom;
using NotifyMe.Domain.Exceptions;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Infrastructure.Models;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class EuroproductShopProductService : IShopProductService<IDocument>
{
    public string Price { get; set; } = null!;
    public string? DiscountedPrice { get; set; }

    public ProductPriceInformation GetPriceInformation(IDocument content)
    {
        var productItem = content.QuerySelector("div.product-item")!;

        var discountedPriceElWhenDiscount = productItem.QuerySelector("span.product-price span.new");
        var regularPriceElWhenDiscount = productItem.QuerySelector("span.product-price span.old");

        if (discountedPriceElWhenDiscount != null && regularPriceElWhenDiscount != null)
        {
            DiscountedPrice = discountedPriceElWhenDiscount.TextContent;
            Price = regularPriceElWhenDiscount.TextContent;
        }
        else
        {
            Price = content
                .QuerySelector("span.product-price > span")
                ?.TextContent!;
        }

        return new ProductPriceInformation()
        {
            DiscountedPrice = DiscountedPrice?.NormalizePrice(),
            Price = Price.NormalizePrice(),
            IsDiscounted = DiscountedPrice != null
        };
    }

    public string GetProductName(IDocument content)
    {
        var element = content.QuerySelector("meta[property='og:title']");
        return element?.GetAttribute("content") ?? throw new ValidationException("Wrong Domain");
    }
}
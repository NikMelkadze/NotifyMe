using AngleSharp.Dom;
using NotifyMe.Domain.Exceptions;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Infrastructure.Models;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class ItWorksShopProductService : IShopProductService<IDocument>
{
    public string Price { get; set; } = null!;
    public string? DiscountedPrice { get; set; }

    public ProductPriceInformation GetPriceInformation(IDocument content)
    {
        var pricesDiv = content.QuerySelector("div.prices-container");

        var regularPriceWhenDiscrount = pricesDiv?.QuerySelector("#old-price")?.TextContent;

        if (regularPriceWhenDiscrount != null)
        {
            Price = regularPriceWhenDiscrount;
            DiscountedPrice = pricesDiv?.QuerySelector("#current-price")?.TextContent;
        }
        else
        {
            Price = pricesDiv?.QuerySelector("#current-price")?.TextContent!;
        }

        return new ProductPriceInformation
        {
            IsDiscounted = DiscountedPrice != null,
            DiscountedPrice = DiscountedPrice?.NormalizePrice(),
            Price = Price.NormalizePrice()
        };
    }

    public string GetProductName(IDocument content)
    {
        var element = content.QuerySelector("meta[property='og:title']");
        return element?.GetAttribute("content") ?? throw new ValidationException("Wrong Domain");
    }
}
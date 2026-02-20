using AngleSharp.Dom;
using NotifyMe.Domain.Exceptions;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Infrastructure.Models;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class DressUpShopProductService : IShopProductService<IDocument>
{
    public string Price { get; set; }
    public string? DiscountedPrice { get; set; }

    public ProductPriceInformation GetPriceInformation(IDocument content)
    {
        var pricesRoot = content.QuerySelector(".product-prices");

        var regularPriceWhenDiscrount = pricesRoot?
            .QuerySelector(".product-discount .regular-price")
            ?.TextContent;

        var discountedPriceWhenDiscount = pricesRoot?
            .QuerySelector(".current-price .product-price[itemprop='price']")
            ?.GetAttribute("content");

        if (regularPriceWhenDiscrount != null)
        {
            DiscountedPrice = discountedPriceWhenDiscount!;
            Price = regularPriceWhenDiscrount;
        }
        else
        {
            Price = discountedPriceWhenDiscount!;
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
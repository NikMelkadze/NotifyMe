using AngleSharp.Dom;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Infrastructure.Models;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class AgrohubProductServicec : IShopProductService<IDocument>
{
    public string Price { get; set; } = null!;
    public string? DiscountedPrice { get; set; }

    public ProductPriceInformation GetPriceInformation(IDocument content)
    {
        var priceBlock = content.QuerySelector("p.sc-24adf2a9-19");

        var regularPriceWhenDiscrount = priceBlock!.QuerySelector("span");

        if (regularPriceWhenDiscrount != null)
        {
            Price = regularPriceWhenDiscrount.TextContent;

            var discountedPriceRaw = priceBlock
                .ChildNodes
                .Where(n => n.NodeType == AngleSharp.Dom.NodeType.Text)
                .Select(n => n.TextContent)
                .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));

            DiscountedPrice = discountedPriceRaw;
        }
        else
        {
            Price = priceBlock.TextContent;
        }

        return new ProductPriceInformation()
        {
            DiscountedPrice = DiscountedPrice?.Normalize(),
            Price = Price.NormalizePrice(),
            IsDiscounted = DiscountedPrice != null
        };
    }

    public string GetProductName(IDocument content)
    {
        return content
            .QuerySelector("div.sc-24adf2a9-8 h2")
            ?.TextContent
            ?.Trim()!;
    }
}
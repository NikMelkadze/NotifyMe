using AngleSharp.Dom;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Infrastructure.Models;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class AgrohubProductServicec : IShopProductService<IDocument>
{
    public ProductPriceInformation GetPriceInformation(IDocument content)
    {
        string? currentPrice = null;
        string? oldPrice = null;

        var priceBlock = content.QuerySelector("p.sc-24adf2a9-19");
        var oldPriceEl = priceBlock!.QuerySelector("span");

        if (oldPriceEl != null)
        {
            oldPrice = oldPriceEl.TextContent.NormalizePrice();

            var newPriceRaw = priceBlock
                .ChildNodes
                .Where(n => n.NodeType == AngleSharp.Dom.NodeType.Text)
                .Select(n => n.TextContent)
                .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));

            currentPrice = newPriceRaw.NormalizePrice();
        }
        else
        {
            currentPrice = priceBlock.TextContent.NormalizePrice();
        }

        return new ProductPriceInformation()
        {
            CurrentPrice = currentPrice,
            OldPrice = oldPrice,
            IsDiscounted = oldPrice != null
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
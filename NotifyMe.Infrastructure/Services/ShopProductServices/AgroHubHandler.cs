using AngleSharp;
using AngleSharp.Dom;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Infrastructure.Models;
using NotifyMe.Infrastructure.Services.Common;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class AgroHubHandler(IHttpClientService httpClientService, IBrowsingContext context)
    : ShopHandlerBase(httpClientService, context)
{
    public override async Task<ProductInformation> GetProductInformation(string url,
        CancellationToken cancellationToken)
    {
        var document = await GetDocument(url, cancellationToken);

        var priceBlock = document.QuerySelector("p.sc-24adf2a9-19");

        var regularPriceWhenDiscount = priceBlock!.QuerySelector("span");

        if (regularPriceWhenDiscount != null)
        {
            Price = Convert.ToDecimal(regularPriceWhenDiscount.TextContent.NormalizePrice());

            var discountedPriceRaw = priceBlock
                .ChildNodes
                .Where(n => n.NodeType == NodeType.Text)
                .Select(n => n.TextContent)
                .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));

            DiscountedPrice = Convert.ToDecimal(discountedPriceRaw!.NormalizePrice());
        }
        else
        {
            Price = Convert.ToDecimal(priceBlock.TextContent.NormalizePrice());
        }

        return new ProductInformation
        {
            DiscountedPrice = DiscountedPrice,
            Price = Price,
            IsDiscounted = DiscountedPrice != null,
            Name = GetProductName(document)
        };
    }

    private new static string GetProductName(IDocument content)
    {
        return content
            .QuerySelector("div.sc-24adf2a9-8 h2")
            ?.TextContent.Trim()!;
    }
}
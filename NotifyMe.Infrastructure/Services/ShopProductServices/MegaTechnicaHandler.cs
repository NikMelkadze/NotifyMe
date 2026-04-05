using AngleSharp;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Infrastructure.Models;
using NotifyMe.Infrastructure.Services.Common;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class MegaTechnicaHandler(IHttpClientService httpClientService, IBrowsingContext browsingContext) : ShopHandlerBase(httpClientService, browsingContext)
{
    public override async Task<ProductInformation> GetProductInformation(string url, CancellationToken cancellationToken)
    {
        var document = await GetDocument(url, cancellationToken);
        var pricesDivMega = document.QuerySelector("div.prices");
        var oldPrice = pricesDivMega!.QuerySelector("span.prev_price")?.TextContent.NormalizePrice() ?? "";
        var currentPrice = pricesDivMega!.QuerySelector("span.price")?.TextContent.NormalizePrice() ?? "";
        var isDiscounted = oldPrice != "";

        return new ProductInformation
        {
            IsDiscounted = isDiscounted,
            DiscountedPrice = currentPrice,
            Price = oldPrice,
            Name = GetProductName(document)
        };
    }
}
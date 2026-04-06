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
        Price = Convert.ToDecimal(pricesDivMega!.QuerySelector("span.prev_price")?.TextContent.NormalizePrice());
        DiscountedPrice = Convert.ToDecimal(pricesDivMega!.QuerySelector("span.price")?.TextContent.NormalizePrice());

        //When item is not discounted
        if (Price == 0)
        {
            Price = DiscountedPrice.Value;
            DiscountedPrice = null;
        }
        
        var isDiscounted = DiscountedPrice != null ;

        return new ProductInformation
        {
            IsDiscounted = isDiscounted,
            DiscountedPrice = DiscountedPrice,
            Price = Price,
            Name = GetProductName(document)
        };
    }
}
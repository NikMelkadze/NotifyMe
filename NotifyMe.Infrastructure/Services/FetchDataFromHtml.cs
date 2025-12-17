using AngleSharp;
using NotifyMe.Domain.Enums;
using NotifyMe.Infrastructure.Contracts;
using ValidationException = NotifyMe.Domain.Exceptions.ValidationException;

namespace NotifyMe.Infrastructure.Services;

public class FetchDataFromHtml(IBrowsingContext browsingContext) : FetchDataFactory<string>
{
    public override async Task<(bool isDiscounted, string currentPrice, string prevPrice)> GetDiscountInformation(string product, Shop shop,CancellationToken cancellationToken)
    {
        var document = await browsingContext.OpenAsync(req => req.Content(product), cancellationToken);
        
        if (shop == Shop.Megatechnica)
        {
            var pricesDivMega = document.QuerySelector("div.prices");
            var prevPrice = pricesDivMega!.QuerySelector("span.prev_price")?.TextContent.Trim() ?? "";
            var prevPriceTrimmed = System.Text.RegularExpressions.Regex.Replace(prevPrice, @"[^\d]", "");
            var currentPrice = pricesDivMega!.QuerySelector("span.price")?.TextContent.Trim() ?? "";
            var isDiscounted = prevPrice != "";

            return (isDiscounted, currentPrice, prevPriceTrimmed);
        }

        if (shop == Shop.Alta)
        {
            var currentAlta = document.QuerySelector(".ty-price-num")?.TextContent.Trim() ?? "";
            var prevPriceAlta = document.QuerySelector(".ty-list-price.ty-nowrap")?.TextContent ?? "";
            var prevPriceTrimmed = System.Text.RegularExpressions.Regex.Replace(prevPriceAlta, @"[^\d]", "");
            var isDiscounted = prevPriceAlta != "";

            return (isDiscounted, currentAlta, prevPriceTrimmed);
        }

        throw new NotImplementedException();
    }

    public override async Task<string> GetProductName(string product, Shop shop,CancellationToken cancellationToken)
    {
        var document = await browsingContext.OpenAsync(req => req.Content(product), cancellationToken);
        
        if (shop == Shop.Megatechnica)
        {
            var element = document.QuerySelector("meta[property='og:title']");
            return element?.GetAttribute("content") ?? throw new ValidationException("Wrong Domain");
        }

        throw new NotImplementedException();
    }
}
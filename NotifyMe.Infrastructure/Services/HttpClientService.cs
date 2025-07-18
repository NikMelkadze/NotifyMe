using System.ComponentModel.DataAnnotations;
using AngleSharp;
using NotifyMe.Domain.Enums;
using NotifyMe.Infrastructure.Contracts;

namespace NotifyMe.Infrastructure.Services;

public class HttpClientService : IHttpClientService
{
    private readonly IBrowsingContext _browsingContext;

    public HttpClientService()
    {
        var configuration = Configuration.Default;
        _browsingContext = BrowsingContext.New(configuration);
    }

    public async Task<string> FetchHtmlFromWeb(string url)
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching HTML for product {url} : {ex.Message}");
        }
    }

    public async Task<(bool isDiscounted, string currentPrice, string prevPrice)> GetPriceElements(string html, Shops shop,CancellationToken stoppingToken)
    {
        var document = await _browsingContext.OpenAsync(req => req.Content(html), stoppingToken);
        
        if (shop == Shops.Megatechnica)
        {
            var pricesDivMega = document.QuerySelector("div.prices");
            var prevPrice = pricesDivMega!.QuerySelector("span.prev_price")?.TextContent.Trim() ?? "";
            var prevPriceTrimmed = System.Text.RegularExpressions.Regex.Replace(prevPrice, @"[^\d]", "");
            var currentPrice = pricesDivMega!.QuerySelector("span.price")?.TextContent.Trim() ?? "";
            var isDiscounted = prevPrice != "";

            return (isDiscounted, currentPrice, prevPriceTrimmed);
        }

        if (shop == Shops.Alta)
        {
            var currentAlta = document.QuerySelector(".ty-price-num")?.TextContent.Trim() ?? "";
            var prevPriceAlta = document.QuerySelector(".ty-list-price.ty-nowrap")?.TextContent ?? "";
            var prevPriceTrimmed = System.Text.RegularExpressions.Regex.Replace(prevPriceAlta, @"[^\d]", "");
            var isDiscounted = prevPriceAlta != "";

            return (isDiscounted, currentAlta, prevPriceTrimmed);
        }

        if (shop == Shops.Ee)
        {
        }

        throw new NotImplementedException();
    }

    public async Task<string> GetProductName(string html, Shops shop,CancellationToken stoppingToken)
    {
        var document = await _browsingContext.OpenAsync(req => req.Content(html), stoppingToken);
        
        if (shop == Shops.Megatechnica)
        {
            var element = document.QuerySelector("meta[property='og:title']");
            return element?.GetAttribute("content") ?? throw new ValidationException("Wrong Domain");
        }

        throw new NotImplementedException();
    }
}
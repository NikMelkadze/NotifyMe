using AngleSharp.Dom;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Infrastructure.Contracts;

public interface IHttpClientService
{
    public Task<string> FetchHtmlFromWeb(string url);
    public Task<(bool isDiscounted, string currentPrice, string prevPrice)> GetPriceElements(string html, Shop shop, CancellationToken stoppingToken);
    public Task<string> GetProductName(string html, Shop shop,CancellationToken stoppingToken);

}
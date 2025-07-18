using AngleSharp.Dom;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Infrastructure.Contracts;

public interface IHttpClientService
{
    public Task<string> FetchHtmlFromWeb(string url);
    public Task<(bool isDiscounted, string currentPrice, string prevPrice)> GetPriceElements(string html, Shops shop, CancellationToken stoppingToken);
    public Task<string> GetProductName(string html, Shops shop,CancellationToken stoppingToken);

}
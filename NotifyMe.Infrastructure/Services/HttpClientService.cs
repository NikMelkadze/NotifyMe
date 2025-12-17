using System.Net.Http.Json;
using NotifyMe.Application.Helpers;
using NotifyMe.Domain.Exceptions;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Models.ApiResponse;

namespace NotifyMe.Infrastructure.Services;

public class HttpClientService : IHttpClientService
{
    public async Task<string> GetHtml(string url, CancellationToken cancellationToken)
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

            var response = await client.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching HTML for product {url} : {ex.Message}");
        }
    }

    public async Task<ProductBase> GetProductJson(string url, CancellationToken cancellationToken)
    {
        try
        {
            var apiUrl = ConvertToApiUrl(url);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

            var response = await client.GetAsync(apiUrl, cancellationToken);
            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<ProductBase>(cancellationToken: cancellationToken))!;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching HTML for product {url} : {ex.Message}");
        }
    }

    private string ConvertToApiUrl(string url)
    {
        var productId = UrlHelpers.GetProductId(url);
        var productUrl = productId + UrlHelpers.GetPathAfterDomain(url);

        var domain = UrlHelpers.GetSecondLevelDomain(url);

        switch (domain)
        {
            case "Zoommer":
                return $"https://api.zoommer.ge/v1/Products/details?productId={productId}&url={productUrl}";
            case "Ee":
                return $"https://ee-api.ee.ge/v1/Products/details?productId={productId}&url={productUrl}";
            default: throw new ValidationException("Could not convert to Api url");
        }
    }
}
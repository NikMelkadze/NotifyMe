using System.Net.Http.Json;
using NotifyMe.Application.Helpers;
using NotifyMe.Domain.Exceptions;
using NotifyMe.Infrastructure.Contracts;

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

    public async Task<string> GetProductJson(string url, CancellationToken cancellationToken)
    {
        try
        {
            var apiUrl = ConvertToApiUrl(url);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            client.DefaultRequestHeaders.Add("authorization","Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IkZBNjEwNzA1NDFDODNFQjNFMTQzODVDODA1Q0MwNjcyNEY1RjkyMjZSUzI1NiIsInR5cCI6ImF0K2p3dCIsIng1dCI6Ii1tRUhCVUhJUHJQaFE0WElCY3dHY2s5ZmtpWSJ9.eyJuYmYiOjE3NzUyMzczMzksImV4cCI6MjA5MDU5NzMzOSwiaXNzIjoiaHR0cHM6Ly9lZS1hcGkuZWUuZ2UvIiwiYXVkIjoiQXBpIiwiY2xpZW50X2lkIjoiRWxpdEVsZWN0cm9uaWNzV2ViIiwianRpIjoiOUY3RUUyQTJCRDRBQzVDNDJDQzFBMEU5NkI1RkQ3NjUiLCJpYXQiOjE3NzUyMzczMzksInNjb3BlIjpbIkVsaXRFbGVjdHJvbmljc0FwaSJdfQ.K_DigGHuoqn3DCmAILrxN8hyueRj2ir5ueNQIQo3pR7VMX44hz81zzk_oR5MYWvAvVqhEeDQG9lL3ql06qptUaqus4bVuwYGc9GmGwmzUKYxjIxC5q9UGwTUO1mfwRAB-n-vJcILSMd2x6pocGzvfuGiaM072RpN7zWzKXfH7i5kTRSIXxkqDyYSpYulhRDTG5cxnGIOte-Fj1v8t7Ic4Z5180-HRibtChz5xwa8jjXdplk31WcduRmvROT3u48kXk5pYygDjijJJLUVlcQbwx_QmRwbYI-mhJubuMBT-j5UIE7iTuBHyPLbuGaNW-Vqt88qBYXrfQFqIKTKFIr1GA");

            var response = await client.GetAsync(apiUrl, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(cancellationToken);
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
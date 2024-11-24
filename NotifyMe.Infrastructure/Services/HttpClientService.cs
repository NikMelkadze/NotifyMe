using NotifyMe.Infrastructure.Contracts;

namespace NotifyMe.Infrastructure.Services;

public class HttpClientService : IHttpClientService
{
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
            throw new Exception($"Error fetching HTML: {ex.Message}");
        }
    }
}
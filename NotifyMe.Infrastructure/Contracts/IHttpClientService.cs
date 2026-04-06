namespace NotifyMe.Infrastructure.Contracts;

public interface IHttpClientService
{
    public Task<string> GetHtml(string url, CancellationToken cancellationToken);
    public Task<string> GetProductJson(string url, string jwtToken, CancellationToken cancellationToken);
}
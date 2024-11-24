namespace NotifyMe.Infrastructure.Contracts;

public interface IHttpClientService
{
    public Task<string> FetchHtmlFromWeb(string url);
}
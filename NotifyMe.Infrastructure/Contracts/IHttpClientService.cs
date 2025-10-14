using NotifyMe.Domain.Enums;
using NotifyMe.Infrastructure.Models.ApiResponse;

namespace NotifyMe.Infrastructure.Contracts;

public interface IHttpClientService
{
    public Task<string> GetHtml(string url,CancellationToken cancellationToken);
    public Task<ProductBase> GetProductJson(string url,CancellationToken cancellationToken);
    
}
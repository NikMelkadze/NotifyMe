namespace NotifyMe.Application.Contracts;

public interface ICatalogService
{
    Task<string[]> GetShops(CancellationToken cancellationToken);
}
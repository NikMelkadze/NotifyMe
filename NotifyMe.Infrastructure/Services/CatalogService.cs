using Microsoft.EntityFrameworkCore;
using NotifyMe.Application.Contracts;
using NotifyMe.Persistence;

namespace NotifyMe.Infrastructure.Services;

public class CatalogService(ApplicationDbContext applicationDbContext) : ICatalogService
{
    public async Task<string[]> GetShops(CancellationToken cancellationToken)
    {
        return await applicationDbContext.Shop.Select(x => x.Name).AsNoTracking().ToArrayAsync(cancellationToken);
    }
}
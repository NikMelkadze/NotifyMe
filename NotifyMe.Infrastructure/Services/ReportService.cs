using Microsoft.EntityFrameworkCore;
using NotifyMe.Application.Contracts;
using NotifyMe.Application.Models;
using NotifyMe.Domain.Enums;
using NotifyMe.Persistence;

namespace NotifyMe.Infrastructure.Services;

public class ReportService(ApplicationDbContext dbContext) : IReportService
{
    public async Task<IEnumerable<SavedProduct>> GetTopSavedProducts(int top, CancellationToken cancellationToken)
    {
        return await dbContext.UserSavedProducts
            .Where(r => r.IsActive)
            .GroupBy(r => new { r.Name, r.Shop })
            .Select(g => new SavedProduct
            {
                Name = g.Key.Name,
                Shop = g.Key.Shop.ToString(),
                Count = g.Count()
            }).Take(top)
            .OrderByDescending(x => x.Count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SavedProduct>> GetCompanyTopSavedProducts(int top, Shop shop, CancellationToken cancellationToken)
    {
        return await dbContext.UserSavedProducts
            .Where(r => r.Shop==shop && r.IsActive )
            .GroupBy(r => new { r.Name, r.Shop })
            .Select(g => new SavedProduct
            {
                Name = g.Key.Name,
                Shop = g.Key.Shop.ToString(),
                Count = g.Count()
            }).Take(top)
            .OrderByDescending(x => x.Count)
            .ToListAsync(cancellationToken);
    }
}
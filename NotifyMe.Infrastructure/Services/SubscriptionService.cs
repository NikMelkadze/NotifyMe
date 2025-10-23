using Microsoft.EntityFrameworkCore;
using NotifyMe.Application.Contracts;
using NotifyMe.Persistence;

namespace NotifyMe.Infrastructure.Services;

public class SubscriptionService(ApplicationDbContext dbContext) : ISubscriptionService
{
    public async Task<bool> IsUserSubscribed(long userId,CancellationToken cancellationToken)
    {
        return await dbContext.Subscriptions
            .AsNoTracking()
            .AnyAsync(s => s.UserId == userId && 
                           s.EndDate > DateTime.UtcNow,cancellationToken);
    }
}
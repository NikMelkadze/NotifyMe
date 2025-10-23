namespace NotifyMe.Application.Contracts;

public interface ISubscriptionService
{
    Task<bool> IsUserSubscribed(long userId,CancellationToken cancellationToken);
}
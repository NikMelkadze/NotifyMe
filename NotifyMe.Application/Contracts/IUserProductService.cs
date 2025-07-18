using NotifyMe.Domain.Entities;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Application.Contracts;

public interface IUserProductService
{
    public Task SaveProduct(string url,int userId, NotificationTypes notificationType,CancellationToken cancellationToken);
    public Task<IEnumerable<UserSavedProduct>> GetProducts(int userId,CancellationToken cancellationToken);
}
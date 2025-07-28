using NotifyMe.Domain.Entities;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Application.Contracts;

public interface IUserProductService
{
    public Task SaveProduct(string url, int userId, NotificationType notificationType,
        CancellationToken cancellationToken);

    public Task<IEnumerable<UserSavedProduct>> GetProducts(int userId, CancellationToken cancellationToken);
    public Task DeleteProduct(int productId, int userId, CancellationToken cancellationToken);

    public Task EditProduct(int productId, int userId, bool? isActive, NotificationType? notificationType,
        CancellationToken cancellationToken);
}
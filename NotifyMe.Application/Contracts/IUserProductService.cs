using NotifyMe.Domain.Entities;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Application.Contracts;

public interface IUserProductService
{
    Task SaveProduct(string url, int userId, NotificationType notificationType,
        CancellationToken cancellationToken);

    Task<IEnumerable<UserSavedProduct>> GetProducts(int userId, CancellationToken cancellationToken);
    Task DeleteProduct(int productId, int userId, CancellationToken cancellationToken);

    Task EditProduct(int productId, int userId, bool? isActive, NotificationType? notificationType,
        CancellationToken cancellationToken);

    Task<bool> CanAddProduct(int userId, int productLimit, CancellationToken cancellationToken);
}
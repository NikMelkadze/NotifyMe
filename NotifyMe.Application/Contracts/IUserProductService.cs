using NotifyMe.Application.Models.UserProducts;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Application.Contracts;

public interface IUserProductService
{
    public Task SaveProduct(string url, int userId, NotificationType notificationType,
        CancellationToken cancellationToken);

    public Task<IEnumerable<UserSavedProductResponse>> GetProducts(int userId, bool hasChangedPrice, bool isActive,
        CancellationToken cancellationToken);

    public Task DeleteProduct(int productId, int userId, CancellationToken cancellationToken);

    public Task EditProduct(int productId, int userId, bool? isActive, NotificationType? notificationType,
        CancellationToken cancellationToken);
}
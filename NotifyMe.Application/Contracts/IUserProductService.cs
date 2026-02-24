using NotifyMe.Application.Models.UserProducts;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Application.Contracts;

public interface IUserProductService
{
    public Task SaveProduct(string url, int userId, NotificationType notificationType,
        CancellationToken cancellationToken);

    public Task<UserSavedProductsResponse> GetProducts(int userId, bool hasChangedPrice, ProductStatus? status,
        CancellationToken cancellationToken);

    public Task DeleteProduct(int productId, int userId, CancellationToken cancellationToken);

    public Task EditProduct(int productId, int userId, ProductStatus? status, NotificationType? notificationType,
        CancellationToken cancellationToken);
}
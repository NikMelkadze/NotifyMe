using NotifyMe.Domain.Enums;
using NotifyMe.Infrastructure.Models;

namespace NotifyMe.Infrastructure.Contracts;

public abstract class FetchDataFactory<T>
{
    public abstract Task<ProductPriceInformation> GetDiscountInformation(T product,
        Shop shop, CancellationToken cancellationToken);

    public abstract Task<string> GetProductName(T product, Shop shop, CancellationToken cancellationToken);
}
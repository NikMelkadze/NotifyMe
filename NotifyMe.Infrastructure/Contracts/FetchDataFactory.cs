using NotifyMe.Domain.Enums;
using NotifyMe.Infrastructure.Models;

namespace NotifyMe.Infrastructure.Contracts;

public abstract class FetchDataFactory<T>
{
    public abstract Task<ProductPriceInformation> GetDiscountInformation(T product,
        string shop, CancellationToken cancellationToken);

    public abstract Task<string> GetProductName(T product, string shop, CancellationToken cancellationToken);
}
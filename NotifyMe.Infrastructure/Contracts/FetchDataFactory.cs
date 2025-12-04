using NotifyMe.Domain.Enums;

namespace NotifyMe.Infrastructure.Contracts;

public abstract class FetchDataFactory<T>
{
    public abstract Task<(bool isDiscounted, string currentPrice, string prevPrice)> GetDiscountInformation(T product,
        Shop shop, CancellationToken cancellationToken);

    public abstract Task<string> GetProductName(T product, Shop shop, CancellationToken cancellationToken);
}
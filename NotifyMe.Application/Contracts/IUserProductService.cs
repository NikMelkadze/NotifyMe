using NotifyMe.Domain.Entities;

namespace NotifyMe.Application.Contracts;

public interface IUserProductService
{
    public Task SaveProduct(string url,int userId,CancellationToken cancellationToken);
    public Task<IEnumerable<UserSavedProduct>> GetProducts(int userId,CancellationToken cancellationToken);
}
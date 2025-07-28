using Microsoft.EntityFrameworkCore;
using NotifyMe.Application.Contracts;
using NotifyMe.Application.Helpers;
using NotifyMe.Domain.Entities;
using NotifyMe.Domain.Enums;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Persistence;

namespace NotifyMe.Infrastructure.Services;

public class UserProductService(ApplicationDbContext dbContext,IHttpClientService httpClientService) : IUserProductService
{
    public async Task SaveProduct(string url, int userId, NotificationType notificationType, CancellationToken cancellationToken)
    {
        var shop = Validators.UrlValidator(url);
        
        var response= await httpClientService.FetchHtmlFromWeb(url);

        var name =await httpClientService.GetProductName(response,shop,cancellationToken);
        
         dbContext.UserSavedProducts.Add(new UserSavedProduct
        {
            IsActive = true,
            Name = name,
            Url = url,
            UserId = userId,
            Shop = shop,
            NotificationType = notificationType
        });
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserSavedProduct>> GetProducts(int userId, CancellationToken cancellationToken )
    {
        return await dbContext.UserSavedProducts.Where(x => x.UserId == userId).AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task DeleteProduct(int productId, int userId, CancellationToken cancellationToken)
    {
        await dbContext.UserSavedProducts.Where(x => x.UserId == userId && x.Id == productId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
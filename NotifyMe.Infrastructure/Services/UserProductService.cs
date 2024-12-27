using NotifyMe.Application.Contracts;
using NotifyMe.Application.Helpers;
using NotifyMe.Domain.Entities;
using NotifyMe.Persistence;

namespace NotifyMe.Infrastructure.Services;

public class UserProductService(ApplicationDbContext dbContext) : IUserProductService
{
    public async Task SaveProduct(string url,int userId)
    {
        Validators.UrlValidator(url);
        
        await dbContext.UserSavedProducts.AddAsync(new UserSavedProduct
        {
            IsActive = true,
            Url = url,
            UserId = userId
            
        });
        await dbContext.SaveChangesAsync();
    }
}
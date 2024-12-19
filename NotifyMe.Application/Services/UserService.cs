using NotifyMe.Application.Contracts;
using NotifyMe.Application.Helpers;
using NotifyMe.Domain.Entities;
using NotifyMe.Persistence;

namespace NotifyMe.Application.Services;

public class UserService(ApplicationDbContext dbContext) : IUserService
{
    public async Task SaveProduct(string url)
    {
        Validators.UrlValidator(url);
        
        await dbContext.UserSavedProducts.AddAsync(new UserSavedProduct
        {
            IsActive = true,
            Url = url
        });
        dbContext.SaveChanges();
    }
}
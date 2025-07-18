using NotifyMe.Application.Contracts;
using NotifyMe.Application.Helpers;
using NotifyMe.Domain.Entities;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Persistence;

namespace NotifyMe.Infrastructure.Services;

public class UserProductService(ApplicationDbContext dbContext,IHttpClientService httpClientService) : IUserProductService
{
    public async Task SaveProduct(string url, int userId)
    {
        var shop = Validators.UrlValidator(url);
        
        var response= await httpClientService.FetchHtmlFromWeb(url);

        var name =await httpClientService.GetProductName(response,shop,CancellationToken.None);
        
        await dbContext.UserSavedProducts.AddAsync(new UserSavedProduct
        {
            IsActive = true,
            Name = name,
            Url = url,
            UserId = userId,
            Shop = shop
        });
        await dbContext.SaveChangesAsync();
    }
}
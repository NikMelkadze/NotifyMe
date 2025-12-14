using AngleSharp;
using Microsoft.EntityFrameworkCore;
using NotifyMe.Application.Contracts;
using NotifyMe.Application.Helpers;
using NotifyMe.Domain.Entities;
using NotifyMe.Domain.Enums;
using NotifyMe.Domain.Exceptions;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Persistence;

namespace NotifyMe.Infrastructure.Services;

public class UserProductService(
    ApplicationDbContext dbContext,
    IHttpClientService httpClientService,
    IBrowsingContext browsingContext)
    : IUserProductService
{
    public async Task SaveProduct(string url, int userId, NotificationType notificationType,
        CancellationToken cancellationToken)
    {
        var shop = Validators.UrlValidator(url);

        string shopName;
        if (shop is Shop.Alta or Shop.Megatechnica)
        {
            var html = await httpClientService.GetHtml(url, cancellationToken);
            var factory = new FetchDataFromHtml(browsingContext);
            shopName = await factory.GetProductName(html, shop, cancellationToken);
        }

        else
        {
            var product = await httpClientService.GetProductJson(url, cancellationToken);
            var factory = new FetchDataFromJson();
            shopName = await factory.GetProductName(product, shop, cancellationToken);
        }


        dbContext.UserSavedProducts.Add(new UserSavedProduct
        {
            Url = url,
            IsActive = true,
            Name = shopName,
            UserId = userId,
            Shop = shop,
            NotificationType = notificationType
        });
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserSavedProduct>> GetProducts(int userId, CancellationToken cancellationToken)
    {
        return await dbContext.UserSavedProducts.Where(x => x.UserId == userId).AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteProduct(int productId, int userId, CancellationToken cancellationToken)
    {
        await dbContext.UserSavedProducts.Where(x => x.UserId == userId && x.Id == productId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task EditProduct(int productId, int userId, bool? isActive, NotificationType? notificationType,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.UserSavedProducts.FirstOrDefaultAsync(
            x => x.UserId == userId && x.Id == productId,
            cancellationToken);

        if (product == null)
        {
            throw new NotFoundException("Product Not found");
        }

        if (isActive != null)
        {
            product.IsActive = isActive.Value;
        }

        if (notificationType != null)
        {
            product.NotificationType = notificationType.Value;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
using AngleSharp;
using Microsoft.EntityFrameworkCore;
using NotifyMe.Application.Contracts;
using NotifyMe.Application.Helpers;
using NotifyMe.Application.Models.UserProducts;
using NotifyMe.Domain.Entities;
using NotifyMe.Domain.Enums;
using NotifyMe.Domain.Exceptions;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Services.ShopProductServices;
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

        var html = await httpClientService.GetHtml(url, cancellationToken);
        var document = await browsingContext.OpenAsync(req => req.Content(html), cancellationToken);

        var factory = new ShopProductFactory();
        var shopFactory = factory.GetShopFactory(shop);

        var priceInformation = shopFactory.GetPriceInformation(document);
        var productName = shopFactory.GetProductName(document);

        var initialPrice = !priceInformation.IsDiscounted
            ? Convert.ToDecimal(priceInformation.CurrentPrice)
            : Convert.ToDecimal(priceInformation.OldPrice);

        // var product = await httpClientService.GetProductJson(url, cancellationToken);
        // var factory = new FetchDataFromJson();
        // productName = await factory.GetProductName(product, shop, cancellationToken);

        dbContext.UserSavedProducts.Add(new UserSavedProduct
        {
            Url = url,
            IsActive = true,
            Name = productName,
            UserId = userId,
            Shop = shop,
            NotificationType = notificationType,
            CreatedAt = DateTime.Now,
            InitialPrice = initialPrice,
            NewPrice = priceInformation.IsDiscounted ? Convert.ToDecimal(priceInformation.CurrentPrice) : null
        });
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserSavedProductResponse>> GetProducts(int userId, bool hasChangedPrice,
        bool isActive,
        CancellationToken cancellationToken)
    {
        var query = dbContext.UserSavedProducts
            .Where(x => x.UserId == userId)
            .AsNoTracking();

        if (hasChangedPrice)
        {
            query = query.Where(x => x.NewPrice != null);
        }

        if (isActive)
        {
            query = query.Where(x => x.IsActive);
        }

        var products = await query
            .ToListAsync(cancellationToken);

        return products.Select(x => new UserSavedProductResponse
        {
            Id = x.Id,
            Name = x.Name,
            NotificationType = x.NotificationType.ToString(),
            IsActive = x.IsActive,
            Shop = x.Shop.ToString(),
            Url = x.Url,
            InitialPrice = x.InitialPrice,
            NewPrice = x.NewPrice,
            PriceDifference = x.InitialPrice - x.NewPrice,
            DiscountPercentage = x.NewPrice != null
                ? (int?)((x.InitialPrice - x.NewPrice.Value) / x.InitialPrice * 100m) + "%"
                : null,
        }).ToList();
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
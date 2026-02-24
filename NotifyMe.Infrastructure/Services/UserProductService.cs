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
    IBrowsingContext browsingContext,
    Microsoft.Extensions.Configuration.IConfiguration configuration)
    : IUserProductService
{
    public async Task SaveProduct(string url, int userId, NotificationType notificationType,
        CancellationToken cancellationToken)
    {
        await ValidateMaxProducts(userId, cancellationToken);
        var shop = Validators.UrlValidator(url);

        var html = await httpClientService.GetHtml(url, cancellationToken);
        var document = await browsingContext.OpenAsync(req => req.Content(html), cancellationToken);

        var factory = new ShopProductFactory();
        var shopFactory = factory.GetShopFactory(shop);

        var priceInformation = shopFactory.GetPriceInformation(document);
        var productName = shopFactory.GetProductName(document);

        var initialPrice = Convert.ToDecimal(priceInformation.Price);

        // var product = await httpClientService.GetProductJson(url, cancellationToken);
        // var factory = new FetchDataFromJson();
        // productName = await factory.GetProductName(product, shop, cancellationToken);

        dbContext.UserSavedProducts.Add(new UserSavedProduct
        {
            Url = url,
            Status = ProductStatus.Active,
            Name = productName,
            UserId = userId,
            Shop = shop,
            NotificationType = notificationType,
            CreatedAt = DateTime.Now,
            InitialPrice = initialPrice,
            RegularPrice = initialPrice,
            DiscountedPrice = Convert.ToDecimal(priceInformation.DiscountedPrice)
        });
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserSavedProductsResponse> GetProducts(int userId, bool hasChangedPrice,
        ProductStatus? status,
        CancellationToken cancellationToken)
    {
        var query = dbContext.UserSavedProducts
            .Where(x => x.UserId == userId)
            .AsNoTracking();

        var activeProductsCount = await dbContext.UserSavedProducts.CountAsync(x =>
            x.UserId == userId && x.Status == ProductStatus.Active, cancellationToken);

        if (hasChangedPrice)
        {
            query = query.Where(x => x.DiscountedPrice != null);
        }

        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }

        var products = await query
            .ToListAsync(cancellationToken);

        return new UserSavedProductsResponse()
        {
            ActiveProductsCount = activeProductsCount,
            Products = products.Select(x => new Product
            {
                Id = x.Id,
                Name = x.Name,
                NotificationType = x.NotificationType.ToString(),
                Status = x.Status,
                Shop = x.Shop.ToString(),
                Url = x.Url,
                InitialPrice = x.InitialPrice,
                DiscountedPrice = x.DiscountedPrice,
                NewPrice = x.RegularPrice,
                PriceDifference = x.DiscountedPrice != null ? Math.Abs(x.InitialPrice - x.DiscountedPrice.Value) : null,
                DiscountPercentage = x.DiscountedPrice != null
                    ? (int?)((x.DiscountedPrice.Value - x.InitialPrice) / x.InitialPrice * 100m) + "%"
                    : null,
            }).OrderByDescending(x => x.Status == ProductStatus.Active).ThenByDescending(x => x.Id).ToList()
        };
    }

    public async Task DeleteProduct(int productId, int userId, CancellationToken cancellationToken)
    {
        await dbContext.UserSavedProducts.Where(x => x.UserId == userId && x.Id == productId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task EditProduct(int productId, int userId, ProductStatus? status, NotificationType? notificationType,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.UserSavedProducts.FirstOrDefaultAsync(
            x => x.UserId == userId && x.Id == productId,
            cancellationToken);

        if (product == null)
        {
            throw new NotFoundException("Product Not found");
        }

        if (status != null)
        {
            if (status.Value == ProductStatus.Active)
            {
                await ValidateMaxProducts(userId, cancellationToken);
                product.Status = ProductStatus.Active;
            }

            else if (status.Value == ProductStatus.Inacetive)
            {
                ResetProductChangedPrices(product);
                product.Status = ProductStatus.Inacetive;
            }
            else
            {
                throw new ValidationException("Wrong Status");
            }
        }

        if (notificationType != null)
        {
            product.NotificationType = notificationType.Value;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task ValidateMaxProducts(int userId, CancellationToken cancellationToken)
    {
        var maxProductCount = configuration.GetSection("MaxProduct");

        var activeProductsCount =
            await dbContext.UserSavedProducts.CountAsync(x => x.UserId == userId && x.Status == ProductStatus.Active,
                cancellationToken);

        if (activeProductsCount >= int.Parse(maxProductCount.Value!))
        {
            throw new ValidationException("Can't add more than active 10 product");
        }
    }

    private void ResetProductChangedPrices(UserSavedProduct product)
    {
        product.RegularPrice = null;
        product.DiscountedPrice = null;
    }
}
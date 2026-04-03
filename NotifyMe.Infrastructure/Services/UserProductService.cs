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
        var shops = await dbContext.Shop.Select(x=> new
        {
            x.Id,
            x.Name
        }).ToListAsync(cancellationToken);
        
        var domain = UrlHelpers.GetSecondLevelDomain(url);
        
        Validators.UrlValidator(domain,shops.Select(x=>x.Name).ToList());

        var html = await httpClientService.GetHtml(url, cancellationToken);
        var document = await browsingContext.OpenAsync(req => req.Content(html), cancellationToken);

        var factory = new ShopProductFactory();
        var shopFactory = factory.GetShopFactory(domain);

        var priceInformation = shopFactory.GetPriceInformation(document);
        var productName = shopFactory.GetProductName(document);

        var initialPrice = Convert.ToDecimal(priceInformation.Price);

        // var product = await httpClientService.GetProductJson(url, cancellationToken);
        // var factory = new FetchDataFromJson();
        // productName = await factory.GetProductName(product, shop, cancellationToken);

        dbContext.SavedProducts.Add(new SavedProduct
        {
            Url = url,
            Status = ProductStatus.Active,
            Name = productName,
            UserId = userId,
            ShopId = shops.Where(x=>x.Name==domain).Select(x=>x.Id).First(),
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
        var query = dbContext.SavedProducts
            .Where(x => x.UserId == userId)
            .AsNoTracking();

        var activeProductsCount = await dbContext.SavedProducts.CountAsync(x =>
            x.UserId == userId && x.Status == ProductStatus.Active, cancellationToken);

        if (hasChangedPrice)
        {
            query = query.Where(x => x.DiscountedPrice != null);
        }

        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }

        var products = await query.Include(savedProduct => savedProduct.Shop)
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
                Shop = x.Shop.Name,
                Url = x.Url,
                InitialPrice = x.InitialPrice,
                DiscountedPrice = x.DiscountedPrice,
                NewPrice = x.RegularPrice,
                PriceDifference = x.DiscountedPrice != null ? Math.Abs(x.InitialPrice - x.DiscountedPrice.Value) : null,
                DiscountPercentage = x.DiscountedPrice != null
                    ? (int?)((x.DiscountedPrice.Value - x.InitialPrice) / x.InitialPrice * 100m) + "%"
                    : null,
            }).ToList()
        };
    }

    public async Task DeleteProduct(int productId, int userId, CancellationToken cancellationToken)
    {
        await dbContext.SavedProducts.Where(x => x.UserId == userId && x.Id == productId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task EditProduct(int productId, int userId, ProductStatus? status, NotificationType? notificationType,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.SavedProducts.FirstOrDefaultAsync(
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

            else if (status.Value == ProductStatus.InActive)
            {
                ResetProductChangedPrices(product);
                product.Status = ProductStatus.InActive;
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
            await dbContext.SavedProducts.CountAsync(x => x.UserId == userId && x.Status == ProductStatus.Active,
                cancellationToken);

        if (activeProductsCount >= int.Parse(maxProductCount.Value!))
        {
            throw new ValidationException("Can't add more than active 10 product");
        }
    }

    private void ResetProductChangedPrices(SavedProduct product)
    {
        product.RegularPrice = null;
        product.DiscountedPrice = null;
    }
}
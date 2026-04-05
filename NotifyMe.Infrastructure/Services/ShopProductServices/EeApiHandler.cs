using System.Globalization;
using System.Text.Json;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Models;
using NotifyMe.Infrastructure.Models.ApiResponse;
using NotifyMe.Infrastructure.Services.Common;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class EeApiHandler(IHttpClientService httpClientService) : ShopHandlerBase(httpClientService, null)
{
    private readonly IHttpClientService _httpClientService = httpClientService;

    public override async Task<ProductInformation> GetProductInformation(string url,
        CancellationToken cancellationToken)
    {
        var content = await _httpClientService.GetProductJson(url, cancellationToken);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var product = JsonSerializer.Deserialize<EeResponse>(content, options);

        if (product!.Product.PreviousPrice != null)
        {
            Price = product.Product.PreviousPrice.ToString()!;
            DiscountedPrice = product!.Product.Price.ToString(CultureInfo.CurrentCulture);
        }

        Price = product!.Product.Price.ToString(CultureInfo.CurrentCulture);


        return new ProductInformation
        {
            Price =Price ,
            DiscountedPrice = DiscountedPrice,
            IsDiscounted = DiscountedPrice != null,
            Name = product.Product.Name,
        };
    }
}
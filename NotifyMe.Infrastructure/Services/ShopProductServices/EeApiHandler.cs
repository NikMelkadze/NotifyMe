using System.Globalization;
using System.Net.Http.Json;
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
        var product = JsonSerializer.Deserialize<EeResponse>(content,options);

        return new ProductInformation
        {
            Price = product!.Product.Price.ToString(CultureInfo.CurrentCulture),
            DiscountedPrice = product.Product.PreviousPrice.ToString(CultureInfo.CurrentCulture),
            IsDiscounted = DiscountedPrice != null,
            Name = product.Product.Name,
        };
    }
}
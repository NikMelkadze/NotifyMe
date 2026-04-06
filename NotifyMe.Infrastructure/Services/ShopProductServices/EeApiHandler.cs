using System.Text.Json;
using Microsoft.Extensions.Options;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Models;
using NotifyMe.Infrastructure.Models.ApiResponse;
using NotifyMe.Infrastructure.Services.Common;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class EeApiHandler(IHttpClientService httpClientService, IOptionsMonitor<JwtTokensOption> tokensOption)
    : ShopHandlerBase(httpClientService, null)
{
    private readonly IHttpClientService _httpClientService = httpClientService;
    private readonly JwtTokensOption _tokensOption = tokensOption.CurrentValue;

    public override async Task<ProductInformation> GetProductInformation(string url,
        CancellationToken cancellationToken)
    {
        var content = await _httpClientService.GetProductJson(url, _tokensOption.Ee, cancellationToken);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var product = JsonSerializer.Deserialize<EeResponse>(content, options);

        if (product!.Product.PreviousPrice != null)
        {
            Price = product.Product.PreviousPrice.Value;
            DiscountedPrice = product!.Product.Price;
        }
        else
        {
            Price = product.Product.Price;
        }

        return new ProductInformation
        {
            Price = Price,
            DiscountedPrice = DiscountedPrice,
            IsDiscounted = DiscountedPrice != null,
            Name = product.Product.Name,
        };
    }
}
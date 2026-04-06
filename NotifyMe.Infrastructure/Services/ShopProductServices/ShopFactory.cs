using AngleSharp;
using Microsoft.Extensions.Options;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Models;
using NotifyMe.Infrastructure.Services.Common;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class ShopFactory(IHttpClientService httpClientService, IBrowsingContext browsingContext, IOptionsMonitor<JwtTokensOption> tokensOption)
{
    private readonly Dictionary<string, ShopHandlerBase> _shopHtmlFactories = new()
    {
        {
            "Megatechnica", new MegaTechnicaHandler(httpClientService, browsingContext)
        },
        {
            "Itworks", new ItWorksHandler(httpClientService, browsingContext)
        },
        {
            "Dressup", new DressUpHandler(httpClientService, browsingContext)
        },
        {
            "Europroduct", new EuroProductHandler(httpClientService, browsingContext)
        },
        {
            "Agrohub", new AgroHubHandler(httpClientService, browsingContext)
        },
        {
            "Ee", new EeApiHandler(httpClientService,tokensOption)
        }
    };

    public ShopHandlerBase GetShopFactory(string shop)
    {
        if (_shopHtmlFactories.TryGetValue(shop, out var factory))
        {
            return factory;
        }

        throw new InvalidOperationException("Shop factory is not implemented yet");
    }
}
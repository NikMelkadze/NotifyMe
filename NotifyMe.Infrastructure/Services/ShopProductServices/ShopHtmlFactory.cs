using AngleSharp;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Services.Common;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class ShopHtmlFactory(IHttpClientService httpClientService, IBrowsingContext browsingContext)
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
            "Ee", new EeApiHandler(httpClientService)
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
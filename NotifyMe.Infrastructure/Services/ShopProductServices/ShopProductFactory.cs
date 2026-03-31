using AngleSharp.Dom;
using NotifyMe.Infrastructure.Contracts;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class ShopProductFactory
{
    private readonly Dictionary<string, IShopProductService<IDocument>> _shopFactories = new()
    {
        {
            "Megatechnica", new MegatechnicaShopProductService()
        },
        {
            "Itworks", new ItWorksShopProductService()
        },
        {
            "Dressup", new DressUpShopProductService()
        },
        {
            "Europroduct", new EuroproductShopProductService()
        },
        {
            "Agrohub", new AgrohubProductServicec()
        }
    };

    public IShopProductService<IDocument> GetShopFactory(string shop)
    {
        if (_shopFactories.TryGetValue(shop, out var factory))
        {
            return factory;
        }

        throw new InvalidOperationException("Shop factory is not implemented yet");
    }
}
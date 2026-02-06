using AngleSharp.Dom;
using NotifyMe.Domain.Enums;
using NotifyMe.Infrastructure.Contracts;

namespace NotifyMe.Infrastructure.Services.ShopProductServices;

public class ShopProductFactory
{
    private readonly Dictionary<Shop, IShopProductService<IDocument>> _shopFactories = new()
    {
        {
            Shop.Megatechnica, new MegatechnicaShopProductService()
        },
        {
            Shop.Itworks, new ItWorksShopProductService()
        },
        {
            Shop.Dressup, new DressUpShopProductService()
        },
        {
            Shop.Europroduct, new EuroproductShopProductService()
        },
        {
            Shop.Agrohub, new AgrohubProductServicec()
        },
        {
            Shop.Agrohub, new AgrohubProductServicec()
        }
    };

    public IShopProductService<IDocument> GetShopFactory(Shop shop)
    {
        if (_shopFactories.TryGetValue(shop, out var factory))
        {
            return factory;
        }

        throw new InvalidOperationException("Shop factory is not implemented yet");
    }
}
using NotifyMe.Application.Contracts;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Infrastructure.Services;

public class CatalogService : ICatalogService
{
    public string[] GetShops()
    {
        return Enum.GetNames<Shop>();
    }
}
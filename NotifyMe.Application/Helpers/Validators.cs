using System.ComponentModel.DataAnnotations;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Application.Helpers;

public static class Validators
{
    public static Shop UrlValidator(string url)
    {
        var domain = UrlHelpers.GetSecondLevelDomain(url);
        var shops = Enum.GetNames<Shop>();

        if (!shops.Contains(domain))
        {
            throw new ValidationException("Wrong Domain");
        }

        return (Shop)Enum.Parse(typeof(Shop), domain);
    }

}
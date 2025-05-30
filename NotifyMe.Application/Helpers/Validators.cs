using System.ComponentModel.DataAnnotations;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Application.Helpers;

public static class Validators
{
    public static Shops UrlValidator(string url)
    {
        var domain = GetSecondLevelDomain(url);
        var shops = Enum.GetNames<Shops>();

        if (!shops.Contains(domain))
        {
            throw new ValidationException("Wrong Domain");
        }

        return (Shops)Enum.Parse(typeof(Shops), domain);
    }

    private static string GetSecondLevelDomain(string url)
    {
        try
        {
            var uri = new Uri(url);
            var host = uri.Host;
            var parts = host.Split('.');

            if (parts.Length < 2) return "Invalid domain format";
            var secondLevelDomain = parts[parts.Length - 2]; // Get the second-to-last part (example)
            return char.ToUpper(secondLevelDomain[0]) + secondLevelDomain.Substring(1);
        }
        catch (UriFormatException)
        {
            return "Invalid URL";
        }
    }
}
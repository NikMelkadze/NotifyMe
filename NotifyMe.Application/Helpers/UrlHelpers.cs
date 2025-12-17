using System.Text.RegularExpressions;
using NotifyMe.Domain.Exceptions;

namespace NotifyMe.Application.Helpers;

public static class UrlHelpers
{
    public static string GetSecondLevelDomain(string url)
    {
        try
        {
            var uri = new Uri(url);
            var host = uri.Host;
            var parts = host.Split('.');

            if (parts.Length < 2) return "Invalid domain format";
            var secondLevelDomain = parts[parts.Length - 2];
            return char.ToUpper(secondLevelDomain[0]) + secondLevelDomain.Substring(1);
        }
        catch (UriFormatException)
        {
            return "Invalid URL";
        }
    }
    
    public static string GetPathAfterDomain(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ValidationException("URL cannot be null or empty.");

        var uri = new Uri(url);

        // Combine path and query if you want full relative part
        return uri.PathAndQuery.TrimStart('/');
    }
    
    public static string GetProductId(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ValidationException("URL cannot be null or empty.");

        var uri = new Uri(url);

        // Use regex to match "...-p1234" pattern at the end of the path
        var match = Regex.Match(uri.AbsolutePath, @"-p(\d+)$", RegexOptions.IgnoreCase);

        if (match.Success)
            return match.Groups[1].Value;

        throw new ValidationException("Product ID not found in URL.");
    }

}
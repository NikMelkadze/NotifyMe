using System.Text.RegularExpressions;

namespace NotifyMe.Infrastructure.Extensions;

public static class StringExtensions
{
    public static string NormalizePrice(this string price)
    {
        return Regex
            .Replace(price.Trim(), @"[^\d,\.]", "")
            .Replace(",", ".");
    }
}
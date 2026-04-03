using NotifyMe.Domain.Exceptions;

namespace NotifyMe.Application.Helpers;

public static class Validators
{
    public static void UrlValidator(string domain, List<string> shops)
    {
        if (!shops.Contains(domain))
        {
            throw new ValidationException("Wrong Domain");
        }
    }
}
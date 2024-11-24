using Microsoft.Extensions.DependencyInjection;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Services;

namespace NotifyMe.Infrastructure.Extensions;

public static class Configurations
{
    public static void InstallApplicationExtensions(this IServiceCollection services)
    {
        services.AddScoped<IHttpClientService, HttpClientService>();
    }
}
using AngleSharp;
using Microsoft.Extensions.DependencyInjection;
using NotifyMe.Application.Contracts;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Models;
using NotifyMe.Infrastructure.Services;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace NotifyMe.Infrastructure.Extensions;

public static class Configurations
{
    public static void AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddSingleton<IBrowsingContext>(sp =>
        {
            var angleSharpConfiguration = Configuration.Default;
            return BrowsingContext.New(angleSharpConfiguration);
        });
        services.AddScoped<IHttpClientService, HttpClientService>();
        services.AddScoped<IUserProductService, UserProductService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICatalogService, CatalogService>();
        
        services.Configure<JwtTokensOption>(configuration.GetSection("JwtTokens"));
    }
}
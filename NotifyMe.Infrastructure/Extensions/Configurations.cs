using Microsoft.Extensions.DependencyInjection;
using NotifyMe.Application.Contracts;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Services;

namespace NotifyMe.Infrastructure.Extensions;

public static class Configurations
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IHttpClientService, HttpClientService>();
        services.AddScoped<IUserProductService, UserProductService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}
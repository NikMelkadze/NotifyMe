using Microsoft.Extensions.DependencyInjection;
using NotifyMe.Application.Contracts;
using NotifyMe.Application.Services;

namespace NotifyMe.Application;

public static class Configuration
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }
}
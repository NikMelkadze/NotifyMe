using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NotifyMe.Persistence;

public static class Configuration
{
    public static void AddDatabase(this IServiceCollection services,string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
    }

}
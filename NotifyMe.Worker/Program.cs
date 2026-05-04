using AngleSharp;
using NotifyMe.Application.Contracts;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Models;
using NotifyMe.Infrastructure.Services;
using NotifyMe.Persistence;
using NotifyMe.Worker;
using Configuration = AngleSharp.Configuration;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDatabase(builder.Configuration.GetConnectionString("ConnStr")!);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IBrowsingContext>(sp =>
{
    var configuration = Configuration.Default;
    return BrowsingContext.New(configuration);
});
builder.Services.AddSingleton<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.Configure<JwtTokensOption>(builder.Configuration.GetSection("JwtTokens"));


var host = builder.Build();
host.Run();
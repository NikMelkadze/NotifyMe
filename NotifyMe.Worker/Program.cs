using AngleSharp;
using NotifyMe.Infrastructure.Contracts;
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

var host = builder.Build();
host.Run();
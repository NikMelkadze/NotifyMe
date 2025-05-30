using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Services;
using NotifyMe.Persistence;
using NotifyMe.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDatabase(builder.Configuration.GetConnectionString("ConnStr")!);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IHttpClientService, HttpClientService>();

var host = builder.Build();
host.Run();
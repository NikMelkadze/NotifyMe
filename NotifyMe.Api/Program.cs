using NotifyMe.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallApplicationExtensions();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapGet("/hello", () => "Hello World!");

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
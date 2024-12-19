using NotifyMe.Application.Helpers;
using NotifyMe.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapPost("/user-item", (string req) => Task.FromResult(Validators.UrlValidator(req)));

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

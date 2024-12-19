using NotifyMe.Application.Helpers;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();
builder.Services.AddDatabase(builder.Configuration.GetConnectionString("ConnStr")!);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.MapPost("/user-item", (string req) => Task.FromResult(Validators.UrlValidator(req)));

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

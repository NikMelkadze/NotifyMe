using NotifyMe.Application;
using NotifyMe.Application.Contracts;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddDatabase(builder.Configuration.GetConnectionString("ConnStr")!);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.MapPost("/user-item", (IUserService userService,string req) => Task.FromResult(
    userService.SaveProduct(req)
    
));

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
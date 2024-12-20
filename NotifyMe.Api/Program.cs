using NotifyMe.Application;
using NotifyMe.Infrastructure.Extensions;
using NotifyMe.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

builder.Services.AddDatabase(builder.Configuration.GetConnectionString("ConnStr")!);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
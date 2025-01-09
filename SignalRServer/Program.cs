using DiscountCodes.SignalRServer.Services;
using DiscountCodes.SignalRServer.Infrastructure;
using DiscountCodes.SignalRServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(
    options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("Default");
        options.UseSqlServer(connectionString);
    }
);

builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5132")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddScoped<IDatabaseContext, DatabaseContext>();
builder.Services.AddScoped<IDiscountCodeService, DiscountCodeService>();

var app = builder.Build();

app.UseRouting();
app.UseCors();

app.MapHub<SignalRHub>("/hub");

app.Run();

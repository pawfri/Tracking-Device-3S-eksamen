using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TrackingDeviceLib.Data;
using TrackingDeviceLib.Models;
using TrackingDeviceLib.Services;
using TrackingDeviceLib.Services.Interfaces;
using TrackingDeviceLib.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<Geocoding>();
builder.Services.AddScoped<ITrackingDeviceRepo, TrackingDeviceDBRepo>();
builder.Services.AddDbContext<TrackingDeviceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(); // Session support
builder.Services.AddHttpContextAccessor(); // Giver adgang til HttpContext

builder.Services.AddScoped<AuthService>(); // login/logud service
builder.Services.AddScoped<IUserDBRepo, UserDBRepo>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("allowAnything", builder =>
        builder.WithOrigins("http://localhost:5500", "http://127.0.0.1:5500") // TODO: SKAL VI ÆNDRE/TILFØJE LINK TIL NÅR VI KØRER WEBAPP ONLINE!
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.MapOpenApi();

app.UseCors("allowAnything");

app.UseSession(); // Aktiver session-håndtering
app.UseAuthorization();

app.MapControllers();

app.Run();

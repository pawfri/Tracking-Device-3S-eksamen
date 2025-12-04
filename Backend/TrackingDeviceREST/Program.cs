using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TrackingDeviceLib.Data;
using TrackingDeviceLib.Models;
using TrackingDeviceLib.Services.Interfaces;
using TrackingDeviceLib.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<GeocodingService>();
builder.Services.AddScoped<ITrackingDeviceRepo, TrackingDeviceDBRepo>();
builder.Services.AddDbContext<TrackingDeviceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


builder.Services.AddCors(options =>
{
    //options.AddPolicy("allowGetPut",
    //builder =>
    //builder.AllowAnyOrigin()
    //.WithMethods("GET", "PUT")
    //.AllowAnyHeader());

    options.AddPolicy("allowAnything",
	builder =>
	builder.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.MapOpenApi();

app.UseCors("allowAnything");

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TrackingDeviceLib.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TrackingDeviceRepo>();

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

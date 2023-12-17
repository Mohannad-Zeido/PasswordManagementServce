using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PasswordManagementService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<PMSDatabaseSettings>(
    builder.Configuration.GetSection("PMSDataBase"));

builder.Services.AddSingleton<PasswordsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapGet("/passwordAdd", async () =>
    {
        var asd = app.Services.GetRequiredService<PasswordsService>();
        await asd.CreateAsync(new PasswordStorage
        {
            Id = "61a6058e6c43f32854e51f51",
            Author = "me",
            Category = "Frank",
            Price = new decimal(1.00),
            BookName = "Senator Frank Bio"
        });
    })
    .WithName("Password Add")
    .WithOpenApi();

app.MapGet("/password", async () =>
    {
        var asd = app.Services.GetRequiredService<PasswordsService>();
        var result = await asd.GetAsync("61a6058e6c43f32854e51f51");
        return result;
    })
    .WithName("GetPassword")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
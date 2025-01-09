using Microsoft.EntityFrameworkCore;
using WebApiApp.Data;
using WebApiApp.Helpers;
using WebApiApp.Repositories;
using WebApiApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext with PostgreSQL connection
var connectionURI = builder.Configuration.GetConnectionString("DefaultConnection");
connectionURI = DbConnectionHelper.BuildConnectionURI(connectionURI);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionURI));

// Configure repositories
builder.Services.AddScoped<AccountRepository>();

// Configure services
builder.Services.AddScoped<AccountService>();

// Add AutoMapper with a custom mapping profile
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
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

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

using Microsoft.EntityFrameworkCore;
using WebApiApp.Middlewares;
using WebApiApp.Data;
using WebApiApp.Helpers;
using WebApiApp.Models;
using WebApiApp.Repositories;
using WebApiApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext with PostgreSQL connection
var DbConnection = builder.Configuration.GetValue<string>("Connection:Postgres");
DbConnection = DbConnectionHelper.BuildPostgresConnection(DbConnection!);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(DbConnection));

// Load SMTP settings from configuration
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("SMTP"));

// Load OAuth settings from configuration
builder.Services.Configure<GoogleAuthSettings>(builder.Configuration.GetSection("OAUTH"));
builder.Services.Configure<FacebookAuthSettings>(builder.Configuration.GetSection("OAUTH"));
builder.Services.Configure<LineAuthSettings>(builder.Configuration.GetSection("OAUTH"));

// Configure repositories
builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<SocialAccountRepository>();

// Configure services
builder.Services.AddHttpClient<HttpRequestService>();
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddTransient<EmailService>();
builder.Services.AddTransient<TemplateService>();
builder.Services.AddTransient<GoogleAuthService>();
builder.Services.AddTransient<FacebookAuthService>();
builder.Services.AddTransient<LineAuthService>();

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

// Custom middlewares
app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();

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

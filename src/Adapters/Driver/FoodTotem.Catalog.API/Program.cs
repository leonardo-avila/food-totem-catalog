using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using FoodTotem.Catalog.Gateways.MySQL.Contexts;
using FoodTotem.Catalog.API.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

// Add services to the container.

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true); 

builder.Services.AddControllers(options =>
    {
        options.Conventions.Add(new DashRouteConvention());
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Configure Swagger options
    c.SwaggerDoc("v1", new OpenApiInfo { 
            Title = "Food Totem Catalog API",
            Version = "v1",
            Description = "Contains the endpoints used to manage the catalog of products for Food Totem.",
        });
    var filePath = Path.Combine(AppContext.BaseDirectory, "FoodTotem.Catalog.API.xml");
    c.IncludeXmlComments(filePath);
});

// Set DbContexts
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// Dependency Injection
builder.Services.AddCatalogServices();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var catalogContext = serviceScope.ServiceProvider.GetService<CatalogContext>();
    catalogContext!.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();

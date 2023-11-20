using FoodTotem.Catalog.UseCase.UseCases;
using FoodTotem.Catalog.Domain.Models;
using FoodTotem.Catalog.Domain.Models.Validators;
using FoodTotem.Catalog.Domain.Repositories;
using FoodTotem.Catalog.Domain.Ports;
using FoodTotem.Catalog.Domain.Services;
using FoodTotem.Catalog.UseCase.Ports;
using FluentValidation;
using FoodTotem.Catalog.Gateways.MySQL.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesColletionExtensions
    {
        public static IServiceCollection AddCatalogServices(
            this IServiceCollection services)
        {
            services.AddScoped<IFoodRepository, FoodRepository>();

            services.AddScoped<IFoodUseCases, FoodUseCases>();

            services.AddScoped<IFoodService, FoodService>();

            services.AddScoped<IValidator<Food>, FoodValidator>();

            return services;
        }
    }
}
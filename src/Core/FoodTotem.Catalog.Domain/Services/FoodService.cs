using FoodTotem.Catalog.Domain.Models;
using FoodTotem.Catalog.Domain.Models.Enums;
using FoodTotem.Catalog.Domain.Ports;
using FoodTotem.Domain.Core;
using FluentValidation;

namespace FoodTotem.Catalog.Domain.Services
{
    public class FoodService : IFoodService
	{
        private readonly IValidator<Food> _foodValidator;

        public FoodService(IValidator<Food> foodValidator)
        {
            _foodValidator = foodValidator;
        }

        public bool IsValidCategory(string category)
        {
            return Enum.IsDefined(typeof(FoodCategory), category);
        }

        public void ValidateFood(Food food)
        {
            var validationResult = _foodValidator.Validate(food);

            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.ToString());
            }
        }
    }
}

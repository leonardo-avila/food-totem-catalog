using FluentValidation;

namespace FoodTotem.Catalog.Domain.Models.Validators
{
	public class FoodValidator : AbstractValidator<Food>
	{
		public FoodValidator()
		{
			RuleFor(f => f.Name).NotNull();
			RuleFor(f => f.Description).NotNull();
			RuleFor(f => f.ImageUrl).NotNull();
			RuleFor(f => f.Price).GreaterThan(0).WithMessage("Food should have a positive price.");
			RuleFor(f => f.Category).IsInEnum();
		}
	}
}
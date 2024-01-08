using FoodTotem.Catalog.UseCase.Ports;
using FoodTotem.Catalog.UseCase.InputViewModels;
using FoodTotem.Catalog.Domain.Models;
using FoodTotem.Catalog.Domain.Models.Enums;
using FoodTotem.Catalog.Domain.Repositories;
using FoodTotem.Domain.Core;
using FoodTotem.Catalog.Domain.Ports;
using FoodTotem.Catalog.UseCase.OutputViewModels;

namespace FoodTotem.Catalog.UseCase.UseCases
{
	public class FoodUseCases : IFoodUseCases
	{
		private readonly IFoodRepository _foodRepository;
		private readonly IFoodService _foodService;

        public FoodUseCases(IFoodRepository foodRepository,
			IFoodService foodService)
		{
			_foodRepository = foodRepository;
			_foodService = foodService;
        }

		public async Task<IEnumerable<FoodOutputViewModel>> GetFoods()
		{
			var foods =  await _foodRepository.GetAll();

			return ProduceFoodViewModelCollection(foods);
		}

		public async Task<FoodOutputViewModel> AddFood(FoodInputViewModel foodViewModel)
		{
			if (_foodService.IsValidCategory(foodViewModel.Category))
			{
                var food = new Food(
                foodViewModel.Name,
                foodViewModel.Description,
                foodViewModel.ImageUrl,
                foodViewModel.Price,
                Enum.Parse<FoodCategory>(foodViewModel.Category)
                );

				_foodService.ValidateFood(food);

                await _foodRepository.Create(food);

				return ProduceFoodViewModel(food);
            }
            else
			{
                throw new DomainException("Invalid food category.");
            }
        }

		public async Task<FoodOutputViewModel> GetFood(Guid id)
		{
            var food = await _foodRepository.Get(id) ?? throw new DomainException("There is no food with this id.");

			return ProduceFoodViewModel(food);
        }

		public async Task<IEnumerable<FoodOutputViewModel>> GetFoodsByCategory(string category)
		{
			if (_foodService.IsValidCategory(category))
			{
				var foods = await _foodRepository.GetFoodsByCategory(Enum.Parse<FoodCategory>(category));

				return ProduceFoodViewModelCollection(foods);
			}
            else
            {
                throw new DomainException("Invalid food category.");
            }
        }

		public async Task<FoodOutputViewModel> UpdateFood(Guid id, FoodInputViewModel foodViewModel)
		{
			var food = await _foodRepository.Get(id) ?? throw new DomainException("There is no food with this id.");

			if (_foodService.IsValidCategory(foodViewModel.Category))
			{
                food.UpdateName(foodViewModel.Name);
                food.UpdateDescription(foodViewModel.Description);
                food.UpdateImageUrl(foodViewModel.ImageUrl);
                food.UpdatePrice(foodViewModel.Price);
                food.UpdateCategory(Enum.Parse<FoodCategory>(foodViewModel.Category));

				_foodService.ValidateFood(food);

                await _foodRepository.Update(food);

				return ProduceFoodViewModel(food);
            }
            else
            {
                throw new DomainException("Invalid food category.");
            }
		}

		public async Task<bool> DeleteFood(Guid id)
		{
			var food = await _foodRepository.Get(id);
			return food is null
				? throw new DomainException("No food found with this id.")
				: await _foodRepository.Delete(food);
		}

        private static IEnumerable<FoodOutputViewModel> ProduceFoodViewModelCollection(IEnumerable<Food> foods)
        {
            foreach (var food in foods)
            {
                yield return ProduceFoodViewModel(food);
            }
        }

        private static FoodOutputViewModel ProduceFoodViewModel(Food food)
		{
			return new FoodOutputViewModel()
			{
				Id = food.Id,
				Category = food.Category.ToString(),
				Description = food.Description,
				ImageUrl = food.ImageUrl,
				Name = food.Name,
				Price = food.Price
			};
		}
	}
}
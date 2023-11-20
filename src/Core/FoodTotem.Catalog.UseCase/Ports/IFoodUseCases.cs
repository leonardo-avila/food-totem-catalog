using FoodTotem.Catalog.UseCase.InputViewModels;
using FoodTotem.Catalog.UseCase.OutputViewModels;

namespace FoodTotem.Catalog.UseCase.Ports
{
	public interface IFoodUseCases
	{
		Task<IEnumerable<FoodOutputViewModel>> GetFoods();
		Task<FoodOutputViewModel> AddFood(FoodInputViewModel foodViewModel);
		Task<FoodOutputViewModel> GetFood(Guid id);
		Task<IEnumerable<FoodOutputViewModel>> GetFoodsByCategory(string category);
		Task<bool> DeleteFood(Guid id);
		Task<FoodOutputViewModel> UpdateFood(Guid id, FoodInputViewModel foodViewModel);
	}
}
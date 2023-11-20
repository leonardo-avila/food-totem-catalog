using FoodTotem.Catalog.Domain.Models;

namespace FoodTotem.Catalog.Domain.Ports
{
	public interface IFoodService
	{
		bool IsValidCategory(string category);
		void ValidateFood(Food food);
	}
}
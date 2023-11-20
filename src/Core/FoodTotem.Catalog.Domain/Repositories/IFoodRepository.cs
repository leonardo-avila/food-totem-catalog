using FoodTotem.Data.Core;
using FoodTotem.Catalog.Domain.Models;
using FoodTotem.Catalog.Domain.Models.Enums;

namespace FoodTotem.Catalog.Domain.Repositories
{
	public interface IFoodRepository : IRepository<Food>
	{
		Task<IEnumerable<Food>> GetFoodsByCategory(FoodCategoryEnum category);
	}
}
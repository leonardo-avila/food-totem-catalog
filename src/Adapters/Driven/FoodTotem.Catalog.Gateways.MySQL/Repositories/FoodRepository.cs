using FoodTotem.Catalog.Domain.Models;
using FoodTotem.Catalog.Domain.Models.Enums;
using FoodTotem.Catalog.Domain.Repositories;
using FoodTotem.Catalog.Gateways.MySQL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FoodTotem.Catalog.Gateways.MySQL.Repositories
{
	public class FoodRepository : RepositoryBase<Food>, IFoodRepository
	{
        protected readonly CatalogContext Db;
        protected readonly DbSet<Food> DbSet;

        public FoodRepository(CatalogContext context) : base (context)
        {
            Db = context;
            DbSet = Db.Set<Food>();
        }

        public async Task<IEnumerable<Food>> GetFoodsByCategory(FoodCategory category)
        {
            return await DbSet.Where(f => f.Category.Equals(category)).ToListAsync();
        }
    }
}
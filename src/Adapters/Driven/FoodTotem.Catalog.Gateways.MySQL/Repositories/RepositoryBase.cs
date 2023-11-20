using FoodTotem.Data.Core;
using FoodTotem.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace FoodTotem.Catalog.Gateways.MySQL.Repositories
{
	public abstract class RepositoryBase<T> : IRepository<T> where T: Entity
	{
		private readonly DbContext _dbContext;

		public RepositoryBase(DbContext dbContext)
		{
			_dbContext = dbContext;
		}

        public virtual async Task<IEnumerable<T>> GetAll() => await _dbContext.Set<T>().ToListAsync();

        public virtual async Task<T> Get(Guid id) => await _dbContext.Set<T>().FindAsync(id);

		public virtual async Task<bool> Create(T entity)
		{
			_dbContext.Set<T>().Add(entity);
			return await Commit();
		}

		public virtual async Task<bool> Update(T entity)
		{
			_dbContext.Set<T>().Update(entity);
            return await Commit();
        }

		public virtual async Task<bool> Delete(T entity)
		{
			_dbContext.Set<T>().Remove(entity);
			return await Commit();
		}

        public void Dispose() => _dbContext.Dispose();

		public virtual async Task<bool> Commit()
		{
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
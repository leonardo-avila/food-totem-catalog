using FoodTotem.Domain.Core;

namespace FoodTotem.Data.Core
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(Guid id);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
    }
}
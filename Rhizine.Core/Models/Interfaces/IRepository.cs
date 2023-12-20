using System.Linq.Expressions;

namespace Rhizine.Core.Models.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);

    Task<IEnumerable<T>> GetAllAsync();

    Task AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}
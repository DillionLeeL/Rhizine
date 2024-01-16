using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Rhizine.Core.Models.Interfaces;
using System.Linq.Expressions;

namespace Rhizine.Core.Models;

public class Repository<T>(DbContext context, IMemoryCache cache) : IRepository<T> where T : class, IEntity
{
    private readonly DbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly DbSet<T> _dbSet = context.Set<T>();
    private readonly IMemoryCache _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    private readonly MemoryCacheEntryOptions _cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> GetByIdCachedAsync(int id)
    {
        return await _cache.GetOrCreateAsync(id, async entry =>
        {
            entry.SetOptions(_cacheOptions);
            return await _dbSet.FindAsync(id);
        });
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        _cache.Remove(entity.Id); // Invalidate cache
    }

    public async Task UpdateAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        _cache.Remove(entity.Id); // Invalidate cache
    }

    public async Task DeleteAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        _cache.Remove(entity.Id); // Invalidate cache
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        // This method does not use caching as it runs queries that can return different results
        return await _dbSet.Where(predicate).ToListAsync();
    }
}
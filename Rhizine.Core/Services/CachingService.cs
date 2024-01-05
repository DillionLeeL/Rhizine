using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Rhizine.Core.Services.Interfaces;
using System.Text.Json;

namespace Rhizine.Core.Services;

/// <summary>
/// Provides caching services with support for both in-memory and distributed caching.
/// This service offers methods for retrieving, setting, checking existence, and removing cached items.
/// It can optionally use a distributed cache as a fallback when the in-memory cache is unavailable.
/// </summary>
public class CachingService : ICachingService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILoggingService _loggingService;
    private readonly IDistributedCache _distributedCache;
    private readonly bool _useDistributedCacheFallback;

    public CachingService(ILoggingService loggingService, IMemoryCache memoryCache, IDistributedCache distributedCache, bool useDistributedCacheFallback = false)
    {
        _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        _useDistributedCacheFallback = useDistributedCacheFallback;
    }

    /// <summary>
    /// Asynchronously retrieves a cached item by its key. If the item is not found in the memory cache,
    /// and if distributed cache fallback is enabled, it attempts to retrieve the item from the distributed cache.
    /// </summary>
    /// <typeparam name="T">The type of the item to retrieve from the cache.</typeparam>
    /// <param name="key">The key of the item to retrieve.</param>
    /// <returns>The cached item if found; otherwise, default value of type T.</returns>
    /// <exception cref="ArgumentException">Thrown when the key is null or whitespace.</exception>
    /// <exception cref="Exception">Propagates any exceptions encountered during the operation.</exception>
    public async Task<T> GetAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

        try
        {
            if (_memoryCache.TryGetValue(key, out T value))
            {
                return value;
            }

            if (_useDistributedCacheFallback)
            {
                var serializedValue = await _distributedCache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(serializedValue))
                {
                    return Deserialize<T>(serializedValue);
                }
            }

            return default;
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex, $"Error retrieving item from cache for key {key}.");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously sets a value in the cache with the specified key.
    /// If distributed cache fallback is enabled, the value is also set in the distributed cache.
    /// </summary>
    /// <typeparam name="T">The type of the item to store in the cache.</typeparam>
    /// <param name="key">The key for the cache item.</param>
    /// <param name="value">The value to store in the cache.</param>
    /// <param name="memoryOptions">Optional. Memory cache entry options.</param>
    /// <param name="distributedOptions">Optional. Distributed cache entry options.</param>
    /// <exception cref="ArgumentException">Thrown when the key is null or whitespace.</exception>
    /// <exception cref="Exception">Propagates any exceptions encountered during the operation.</exception>
    public async Task SetAsync<T>(string key, T value, MemoryCacheEntryOptions? memoryOptions = null, DistributedCacheEntryOptions? distributedOptions = null)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

        try
        {
            _memoryCache.Set(key, value, memoryOptions ?? new MemoryCacheEntryOptions());

            if (_useDistributedCacheFallback)
            {
                var serializedValue = Serialize(value);
                await _distributedCache.SetStringAsync(key, serializedValue, distributedOptions ?? new DistributedCacheEntryOptions());
            }
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex, $"Error checking existence in cache for key {key}.");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously checks whether an item with the specified key exists in the cache.
    /// If the item is not found in the memory cache, and if distributed cache fallback is enabled,
    /// checks the distributed cache.
    /// </summary>
    /// <param name="key">The key of the item to check for existence.</param>
    /// <returns>True if the item exists in the cache; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown when the key is null or whitespace.</exception>
    /// <exception cref="Exception">Propagates any exceptions encountered during the operation.</exception>
    public async Task<bool> ExistsAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

        try
        {
            if (_memoryCache.TryGetValue(key, out _))
            {
                return true;
            }

            if (_useDistributedCacheFallback)
            {
                var value = await _distributedCache.GetStringAsync(key);
                return !string.IsNullOrEmpty(value);
            }

            return false;
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex, $"Error checking existence in cache for key {key}.");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously removes a cached item with the specified key from both the memory and distributed caches.
    /// </summary>
    /// <param name="key">The key of the item to remove.</param>
    /// <exception cref="ArgumentException">Thrown when the key is null or whitespace.</exception>
    /// <exception cref="Exception">Propagates any exceptions encountered during the operation.</exception>
    public async Task RemoveAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

        try
        {
            _memoryCache.Remove(key);

            if (_useDistributedCacheFallback)
            {
                await _distributedCache.RemoveAsync(key);
            }
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex, $"Error removing item from cache for key {key}.");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously invalidates the cache based on application-specific logic.
    /// This method should be implemented with custom logic to clear or refresh cache entries.
    /// </summary>
    public async Task InvalidateCacheAsync()
    {
        // custom implementation based on application-specific cache key management.
        // This could involve clearing all entries, or more complex logic based on cache dependencies.
        throw new NotImplementedException();
    }

    private string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value);
    }

    private T Deserialize<T>(string serializedValue)
    {
        return JsonSerializer.Deserialize<T>(serializedValue);
    }
}
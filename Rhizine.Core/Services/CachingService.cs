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

    /// <summary>
    /// Initializes a new instance of the CachingService class.
    /// </summary>
    /// <param name="loggingService">Provides logging services.</param>
    /// <param name="memoryCache">Represents the in-memory cache.</param>
    /// <param name="distributedCache">Represents the distributed cache.</param>
    /// <param name="useDistributedCacheFallback">Indicates whether to use the distributed cache as a fallback.</param>
    public CachingService(ILoggingService loggingService, IMemoryCache memoryCache, IDistributedCache distributedCache, bool useDistributedCacheFallback = false)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        _useDistributedCacheFallback = useDistributedCacheFallback;
    }

    /// <summary>
    /// Asynchronously retrieves a cached item with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the cached item.</typeparam>
    /// <param name="key">The key for the cached item.</param>
    /// <returns>The cached item if found; otherwise, null.</returns>
    public async Task<T?> GetAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

        if (_memoryCache.TryGetValue(key, out T? value))
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
        _loggingService.LogInformation($"Cache miss for key: {key}");
        return default;
    }

    /// <summary>
    /// Asynchronously sets the value with the specified key in the cache.
    /// </summary>
    /// <typeparam name="T">The type of the item to cache.</typeparam>
    /// <param name="key">The key for the item to cache.</param>
    /// <param name="value">The item to cache.</param>
    /// <param name="memoryOptions">The options for caching in memory.</param>
    /// <param name="distributedOptions">The options for caching in the distributed cache.</param>
    public async Task SetAsync<T>(string key, T value, MemoryCacheEntryOptions? memoryOptions = null, DistributedCacheEntryOptions? distributedOptions = null)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

        _memoryCache.Set(key, value, memoryOptions ?? new MemoryCacheEntryOptions());

        if (_useDistributedCacheFallback)
        {
            var serializedValue = Serialize(value);
            await _distributedCache.SetStringAsync(key, serializedValue, distributedOptions ?? new DistributedCacheEntryOptions());
        }
    }

    /// <summary>
    /// Asynchronously checks if an item with the specified key exists in the cache.
    /// </summary>
    /// <param name="key">The key of the item to check.</param>
    /// <returns>true if the item exists in the cache; otherwise, false.</returns>
    public async Task<bool> ExistsAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

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

    /// <summary>
    /// Asynchronously removes the item with the specified key from the cache.
    /// </summary>
    /// <param name="key">The key of the item to remove.</param>
    public async Task RemoveAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

        _memoryCache.Remove(key);

        if (_useDistributedCacheFallback)
        {
            await _distributedCache.RemoveAsync(key);
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

    private static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value);
    }

    private static T? Deserialize<T>(string serializedValue)
    {
        return JsonSerializer.Deserialize<T>(serializedValue);
    }
}
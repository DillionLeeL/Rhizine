using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Rhizine.Core.Services.Interfaces;
using System.Text.Json;

namespace Rhizine.Core.Services;

public class CachingService : ICachingService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILoggingService _loggingService;
    private readonly IDistributedCache _distributedCache;
    private readonly bool _useDistributedCacheFallback;

    public CachingService(ILoggingService loggingService, IMemoryCache cache, IDistributedCache distributedCache, bool useDistributedCacheFallback = false)
    {
        _loggingService = loggingService;
        _memoryCache = cache;
        _distributedCache = distributedCache;
        _useDistributedCacheFallback = useDistributedCacheFallback;
    }


    public async Task<T> GetAsync<T>(string key)
    {
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
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, MemoryCacheEntryOptions memoryOptions = null, DistributedCacheEntryOptions distributedOptions = null)
    {
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
        }

    }

    public async Task<bool> ExistsAsync(string key)
    {
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
            return false;
        }
    }

    public async Task RemoveAsync(string key)
    {
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
        }
    }

    public async Task InvalidateCacheAsync()
    {
        // custom implementation based on application-specific cache key management.
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

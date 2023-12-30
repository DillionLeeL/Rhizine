using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhizine.Services.Interfaces
{
    public interface ICachingService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, MemoryCacheEntryOptions options = null, DistributedCacheEntryOptions distributedOptions = null);
        Task<bool> ExistsAsync(string key);
        Task RemoveAsync(string key);
        Task InvalidateCacheAsync();
    }
}

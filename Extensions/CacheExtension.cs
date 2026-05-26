using Microsoft.Extensions.Caching.Memory;

namespace TravelMasterApi.Extensions
{
    public class CacheExtension
    {
        private readonly IMemoryCache _memoryCache;
        private static List<string> _Listkeys = new List<string>();

        public CacheExtension(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string cacheKey)
        {
            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out T cacheValue))
                {
                    return cacheValue;
                }
            }
            catch { }
            return default(T);
        }

        public T Get<T>(string cacheKey, T defaultValue = default(T))
        {
            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out T cacheValue))
                {
                    return cacheValue;
                }
            }
            catch { }
            return defaultValue;
        }

        public void Set<T>(string cacheKey, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
        {
            try
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions();

                if (absoluteExpiration.HasValue)
                {
                    cacheEntryOptions.SetAbsoluteExpiration(absoluteExpiration.Value);
                }
                else
                {
                    cacheEntryOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                }
                if (slidingExpiration.HasValue)
                {
                    cacheEntryOptions.SetSlidingExpiration(slidingExpiration.Value);
                }
                else
                {
                    cacheEntryOptions.SetSlidingExpiration(TimeSpan.FromMinutes(3));
                }

                _memoryCache.Set(cacheKey, value, cacheEntryOptions);
                if (!_Listkeys.Contains(cacheKey))
                {
                    _Listkeys.Add(cacheKey);
                }
            }
            catch { }
        }

        public async void Remove(string cacheKey, bool isPrefix = false)
        {
            try
            {
                if (isPrefix)
                {
                    foreach (var key in _Listkeys)
                    {
                        if (key.StartsWith(cacheKey))
                        {
                            _memoryCache.Remove(key);
                            _Listkeys.Remove(key);
                        }
                    }
                }
                else
                {
                    _memoryCache.Remove(cacheKey);
                    if (_Listkeys.Contains(cacheKey))
                    {
                        _Listkeys.Remove(cacheKey);
                    }
                }
            }
            catch { }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using XperiCode.SimpleCache.Internal;

namespace XperiCode.SimpleCache
{
    public static class CacheExtensions
    {
        public static T Get<T>(this ObjectCache cache, string key, Func<T> acquire)
        {
            return Get(cache, key, acquire, (cacheKey, acquiredValue) =>
            {
                cache.Set(cacheKey, acquiredValue, null);
            });
        }

        public static Task<T> GetAsync<T>(this ObjectCache cache, string key, Func<Task<T>> acquireAsync)
        {
            return GetAsync(cache, key, acquireAsync, (cacheKey, acquiredValue) =>
            {
                cache.Set(cacheKey, acquiredValue, null);
            });
        }

        public static T Get<T>(this ObjectCache cache, string key, DateTime expirationDate, Func<T> acquire)
        {
            return Get(cache, key, acquire, (cacheKey, acquiredValue) =>
            {
                cache.Set(cacheKey, acquiredValue, expirationDate);
            });
        }

        public static Task<T> GetAsync<T>(this ObjectCache cache, string key, DateTime expirationDate, Func<Task<T>> acquireAsync)
        {
            return GetAsync(cache, key, acquireAsync, (cacheKey, acquiredValue) =>
            {
                cache.Set(cacheKey, acquiredValue, expirationDate);
            });
        }

        public static T Get<T>(this ObjectCache cache, string key, TimeSpan expirationPeriod, Func<T> acquire)
        {
            return Get(cache, key, acquire, (cacheKey, acquiredValue) =>
            {
                cache.Set(cacheKey, acquiredValue, DateTime.Now.Add(expirationPeriod));
            });
        }

        public static Task<T> GetAsync<T>(this ObjectCache cache, string key, TimeSpan expirationPeriod, Func<Task<T>> acquireAsync)
        {
            return GetAsync(cache, key, acquireAsync, (cacheKey, acquiredValue) =>
            {
                cache.Set(cacheKey, acquiredValue, DateTime.Now.Add(expirationPeriod));
            });
        }

        public static T Get<T>(this ObjectCache cache, string key, FileInfo fileInfo, Func<T> acquire)
        {
            return Get(cache, key, acquire, (cacheKey, acquiredValue) =>
            {
                var policy = new CacheItemPolicy();
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { fileInfo.FullName }));

                cache.Set(cacheKey, acquiredValue, policy);
            });
        }

        public static Task<T> GetAsync<T>(this ObjectCache cache, string key, FileInfo fileInfo, Func<Task<T>> acquireAsync)
        {
            return GetAsync(cache, key, acquireAsync, (cacheKey, acquiredValue) =>
            {
                var policy = new CacheItemPolicy();
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { fileInfo.FullName }));

                cache.Set(cacheKey, acquiredValue, policy);
            });
        }

        public static bool IsSet<T>(this ObjectCache cache, string key)
        {
            string cacheKey = CacheKeyGenerator.GenerateCacheKey<T>(key);
            return cache.IsSet(cacheKey);
        }

        public static void Remove<T>(this ObjectCache cache, string key)
        {
            string cacheKey = CacheKeyGenerator.GenerateCacheKey<T>(key);
            cache.Remove(cacheKey);
        }

        public static void Clear<T>(this ObjectCache cache)
        {
            foreach (var entry in cache.Where(entry => CacheKeyGenerator.IsGeneratedCacheKey<T>(entry.Key)))
            {
                cache.Remove(entry.Key);
            }
        }

        private static T Get<T>(this ObjectCache cache, string key, Func<T> acquire, Action<string, object> set)
        {
            string cacheKey = CacheKeyGenerator.GenerateCacheKey<T>(key);

            if (cache.IsSet(cacheKey))
            {
                var acquiredValue = cache.Get(cacheKey);
                if (acquiredValue is NullObject)
                {
                    return default(T);
                }
                return (T)acquiredValue;
            }

            using (CacheLock.Lock(cacheKey))
            {
                if (cache.IsSet(cacheKey))
                {
                    return (T)cache.Get(cacheKey);
                }

                var acquiredValue = acquire();
                if (acquiredValue != null)
                {
                    set(cacheKey, acquiredValue);
                }
                else
                {
                    set(cacheKey, NullObject.Instance);
                }

                return acquiredValue;
            }
        }

        private static async Task<T> GetAsync<T>(this ObjectCache cache, string key, Func<Task<T>> acquireAsync, Action<string, object> set)
        {
            string cacheKey = CacheKeyGenerator.GenerateCacheKey<T>(key);

            if (cache.IsSet(cacheKey))
            {
                return (T)cache.Get(cacheKey);
            }

            using (await CacheLock.LockAsync(cacheKey))
            {
                if (cache.IsSet(cacheKey))
                {
                    return (T)cache.Get(cacheKey);
                }

                var acquiredValue = await acquireAsync();
                if (acquiredValue != null)
                {
                    set(cacheKey, acquiredValue);
                }
                else
                {
                    set(cacheKey, NullObject.Instance);
                }

                return acquiredValue;
            }
        }

        private static bool IsSet(this ObjectCache cache, string cacheKey)
        {
            return cache.Contains(cacheKey);
        }
    }
}

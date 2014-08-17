using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace XperiCode.SimpleCache
{
    public static class CacheExtensions
    {
        public static T Get<T>(this ObjectCache cache, string key, Func<T> acquire)
        {
            string cacheKey = CacheKeyGenerator.GenerateCacheKey<T>(key);

            if (cache.IsSet(cacheKey))
            {
                return (T)cache.Get(cacheKey);
            }

            using (CacheLock.Lock(cacheKey))
            {
                if (cache.IsSet(cacheKey))
                {
                    return (T)cache.Get(cacheKey);
                }

                var result = acquire();

                cache.Set(cacheKey, result, null);

                return result;
            }
        }

        public static async Task<T> GetAsync<T>(this ObjectCache cache, string key, Func<Task<T>> acquire)
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

                var result = await acquire();

                cache.Set(cacheKey, result, null);

                return result;
            }
        }

        public static T Get<T>(this ObjectCache cache, string key, DateTime expirationDate, Func<T> acquire)
        {
            string cacheKey = CacheKeyGenerator.GenerateCacheKey<T>(key);

            if (cache.IsSet(cacheKey))
            {
                return (T)cache.Get(cacheKey);
            }

            using (CacheLock.Lock(cacheKey))
            {
                if (cache.IsSet(cacheKey))
                {
                    return (T)cache.Get(cacheKey);
                }

                var result = acquire();

                cache.Set(cacheKey, result, expirationDate);

                return result;
            }
        }

        public static async Task<T> GetAsync<T>(this ObjectCache cache, string key, DateTime expirationDate, Func<Task<T>> acquire)
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

                var result = await acquire();

                cache.Set(cacheKey, result, expirationDate);

                return result;
            }
        }

        public static T Get<T>(this ObjectCache cache, string key, TimeSpan expirationPeriod, Func<T> acquire)
        {
            string cacheKey = CacheKeyGenerator.GenerateCacheKey<T>(key);

            if (cache.IsSet(cacheKey))
            {
                return (T)cache.Get(cacheKey);
            }

            using (CacheLock.Lock(cacheKey))
            {
                if (cache.IsSet(cacheKey))
                {
                    return (T)cache.Get(cacheKey);
                }

                var result = acquire();

                cache.Set(cacheKey, result, DateTime.Now.Add(expirationPeriod));

                return result;
            }
        }

        public static async Task<T> GetAsync<T>(this ObjectCache cache, string key, TimeSpan expirationPeriod, Func<Task<T>> acquire)
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

                var result = await acquire();

                cache.Set(cacheKey, result, DateTime.Now.Add(expirationPeriod));

                return result;
            }
        }

        public static T Get<T>(this ObjectCache cache, string key, FileInfo fileInfo, Func<T> acquire)
        {
            string cacheKey = CacheKeyGenerator.GenerateCacheKey<T>(key);

            if (cache.IsSet(cacheKey))
            {
                return (T)cache.Get(cacheKey);
            }

            using (CacheLock.Lock(cacheKey))
            {
                if (cache.IsSet(cacheKey))
                {
                    return (T)cache.Get(cacheKey);
                }

                var result = acquire();

                var policy = new CacheItemPolicy();
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { fileInfo.FullName }));

                cache.Set(cacheKey, result, policy);

                return result;
            }
        }

        public static async Task<T> GetAsync<T>(this ObjectCache cache, string key, FileInfo fileInfo, Func<Task<T>> acquire)
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

                var result = await acquire();

                var policy = new CacheItemPolicy();
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { fileInfo.FullName }));

                cache.Set(cacheKey, result, policy);

                return result;
            }
        }

        private static bool IsSet(this ObjectCache cache, string cacheKey)
        {
            return cache.Contains(cacheKey);
        }
    }
}

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
            string regionName = typeof(T).FullName;

            if (cache.IsSet(key, regionName))
            {
                return (T)cache.Get(key, regionName);
            }

            using (CacheLock.Lock(key, regionName))
            {
                if (cache.IsSet(key, regionName))
                {
                    return (T)cache.Get(key, regionName);
                }

                var result = acquire();

                cache.Set(key, result, null, regionName);

                return result;
            }
        }

        public static async Task<T> GetAsync<T>(this ObjectCache cache, string key, Func<Task<T>> acquire)
        {
            string regionName = typeof(T).FullName;

            if (cache.IsSet(key, regionName))
            {
                return (T)cache.Get(key, regionName);
            }

            using (await CacheLock.LockAsync(key, regionName))
            {
                if (cache.IsSet(key, regionName))
                {
                    return (T)cache.Get(key, regionName);
                }

                var result = await acquire();

                cache.Set(key, result, null, regionName);

                return result;
            }
        }

        public static T Get<T>(this ObjectCache cache, string key, DateTime expirationDate, Func<T> acquire)
        {
            string regionName = typeof(T).FullName;

            if (cache.IsSet(key, regionName))
            {
                return (T)cache.Get(key, regionName);
            }

            using (CacheLock.Lock(key, regionName))
            {
                if (cache.IsSet(key, regionName))
                {
                    return (T)cache.Get(key, regionName);
                }

                var result = acquire();

                cache.Set(key, result, expirationDate, regionName);

                return result;
            }
        }

        public static async Task<T> GetAsync<T>(this ObjectCache cache, string key, DateTime expirationDate, Func<Task<T>> acquire)
        {
            string regionName = typeof(T).FullName;

            if (cache.IsSet(key, regionName))
            {
                return (T)cache.Get(key, regionName);
            }

            using (await CacheLock.LockAsync(key, regionName))
            {
                if (cache.IsSet(key, regionName))
                {
                    return (T)cache.Get(key, regionName);
                }

                var result = await acquire();

                cache.Set(key, result, expirationDate, regionName);

                return result;
            }
        }

        public static T Get<T>(this ObjectCache cache, string key, TimeSpan expirationPeriod, Func<T> acquire)
        {
            string regionName = typeof(T).FullName;

            if (cache.IsSet(key, regionName))
            {
                return (T)cache.Get(key, regionName);
            }

            using (CacheLock.Lock(key, regionName))
            {
                if (cache.IsSet(key, regionName))
                {
                    return (T)cache.Get(key, regionName);
                }

                var result = acquire();

                cache.Set(key, result, DateTime.Now.Add(expirationPeriod), regionName);

                return result;
            }
        }

        public static async Task<T> GetAsync<T>(this ObjectCache cache, string key, TimeSpan expirationPeriod, Func<Task<T>> acquire)
        {
            string regionName = typeof(T).FullName;

            if (cache.IsSet(key, regionName))
            {
                return (T)cache.Get(key, regionName);
            }

            using (await CacheLock.LockAsync(key, regionName))
            {
                if (cache.IsSet(key, regionName))
                {
                    return (T)cache.Get(key, regionName);
                }

                var result = await acquire();

                cache.Set(key, result, DateTime.Now.Add(expirationPeriod), regionName);

                return result;
            }
        }

        public static T Get<T>(this ObjectCache cache, string key, FileInfo fileInfo, Func<T> acquire)
        {
            string regionName = typeof(T).FullName;

            if (cache.IsSet(key, regionName))
            {
                return (T)cache.Get(key, regionName);
            }

            using (CacheLock.Lock(key, regionName))
            {
                if (cache.IsSet(key, regionName))
                {
                    return (T)cache.Get(key, regionName);
                }

                var result = acquire();

                var policy = new CacheItemPolicy();
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { fileInfo.FullName }));

                cache.Set(key, result, policy, regionName);

                return result;
            }
        }

        public static async Task<T> GetAsync<T>(this ObjectCache cache, string key, FileInfo fileInfo, Func<Task<T>> acquire)
        {
            string regionName = typeof(T).FullName;

            if (cache.IsSet(key, regionName))
            {
                return (T)cache.Get(key, regionName);
            }

            using (await CacheLock.LockAsync(key, regionName))
            {
                if (cache.IsSet(key, regionName))
                {
                    return (T)cache.Get(key, regionName);
                }

                var result = await acquire();

                var policy = new CacheItemPolicy();
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { fileInfo.FullName }));

                cache.Set(key, result, policy, regionName);

                return result;
            }
        }

        public static bool IsSet<T>(this ObjectCache cache, string key)
        {
            string regionName = typeof(T).FullName;
            return cache.IsSet(key, regionName);
        }

        private static bool IsSet(this ObjectCache cache, string key, string regionName)
        {
            return cache.Contains(key, regionName);
        }
    }
}

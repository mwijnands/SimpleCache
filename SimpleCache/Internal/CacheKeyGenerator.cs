using System;
using System.Linq;

namespace XperiCode.SimpleCache.Internal
{
    internal static class CacheKeyGenerator
    {
        public static string GenerateCacheKey<T>(string key)
        {
            string cacheKeyPrefix = CreateCacheKeyPrefix<T>();
            return string.Format("{0}[{1}]", cacheKeyPrefix, key);
        }

        public static bool IsGeneratedCacheKey<T>(string cacheKey)
        {
            string cacheKeyPrefix = CreateCacheKeyPrefix<T>();
            return cacheKey.StartsWith(cacheKeyPrefix);
        }
  
        private static string CreateCacheKeyPrefix<T>()
        {
            return string.Format("XperiCode.SimpleCache-[{0}]", typeof(T).FullName);
        }
    }
}

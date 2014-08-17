using System;
using System.Linq;

namespace XperiCode.SimpleCache.Internal
{
    internal static class CacheKeyGenerator
    {
        public static string GenerateCacheKey<T>(string key)
        {
            return string.Format("[{0}][{1}]", typeof(T).FullName, key);
        }
    }
}

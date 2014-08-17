using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using XperiCode.SimpleCache.Internal;
using XperiCode.SimpleCache.Tests.Test;

namespace XperiCode.SimpleCache.Tests.Internal
{
    [TestClass]
    public class CacheKeyGeneratorTests
    {
        [TestMethod]
        public void ShouldGenerateCacheKeyPrefixedWithGenericTypeParameterFullName()
        {
            string personTypeFullName = typeof(Person).FullName;
            string key = "FindPerson";

            string expectedCacheKey = string.Format("[{0}][{1}]", personTypeFullName, key);
            string cacheKey = CacheKeyGenerator.GenerateCacheKey<Person>(key);

            Assert.AreEqual(expectedCacheKey, cacheKey);
        }
    }
}

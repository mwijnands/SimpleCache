using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Caching;
using System;

namespace XperiCode.SimpleCache.Tests
{
    [TestClass]
    public class CacheExtensionsTests
    {
        // TODO: Set up Mock ObjectCache to test how often acquirer was called.

        [TestMethod]
        public void TestMethod1()
        {
            string cacheKey = "FindItem";
            Func<string> acquirer = () => "test" + DateTime.Now.Ticks;

            string cachedItem = MemoryCache.Default.Get(cacheKey, acquirer);
            string cachedItem2 = MemoryCache.Default.Get(cacheKey, acquirer);

            Assert.AreEqual(cachedItem, cachedItem2);
        }

        // TODO: Execute cache get methods concurrently with a long running acquirer method to test cachelock.
    }
}

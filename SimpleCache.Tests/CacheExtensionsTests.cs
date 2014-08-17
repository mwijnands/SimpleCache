using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Runtime.Caching;
using System;
using System.Threading.Tasks;
using XperiCode.SimpleCache.Tests.Test;

namespace XperiCode.SimpleCache.Tests
{
    [TestClass]
    public class CacheExtensionsTests
    {
        #region "Setup"

        private Mock<IAcquirer> CreateAcquirerMock()
        {
            var acquirerMock = new Mock<IAcquirer>();
            var person = new Person { Name = "John Doe", Age = 35 };

            acquirerMock.Setup(a => a.Acquire<Person>()).Returns(person);
            acquirerMock.Setup(a => a.AcquireAsync<Person>()).Returns(Task.FromResult(person));

            return acquirerMock;
        }
  
        private MemoryCache CreateObjectCache()
        {
            return new MemoryCache("test");
        }

        #endregion

        [TestMethod]
        public void GetFromCacheTwiceShouldCallAcquireOnlyOnce()
        {
            var acquirerMock = CreateAcquirerMock();
            var acquirer = acquirerMock.Object;
            var cache = CreateObjectCache();
            string cacheKey = "FindPerson";

            cache.Get(cacheKey, acquirer.Acquire<Person>);
            cache.Get(cacheKey, acquirer.Acquire<Person>);

            acquirerMock.Verify(a => a.Acquire<Person>(), Times.Once);
        }

        [TestMethod]
        public async Task GetFromCacheSyncAndAsyncShouldCallAcquireOnlyOnce()
        {
            var acquirerMock = CreateAcquirerMock();
            var acquirer = acquirerMock.Object;
            var cache = CreateObjectCache();
            string cacheKey = "FindPerson";

            cache.Get(cacheKey, acquirer.Acquire<Person>);
            await cache.GetAsync(cacheKey, acquirer.AcquireAsync<Person>);

            acquirerMock.Verify(a => a.Acquire<Person>(), Times.Once);
        }

        [TestMethod]
        public void GetFromCacheTwiceShouldReturnSameObject()
        {
            var acquirerMock = CreateAcquirerMock();
            var acquirer = acquirerMock.Object;
            var cache = CreateObjectCache();
            string cacheKey = "FindPerson";

            var person1 = cache.Get(cacheKey, acquirer.Acquire<Person>);
            var person2 = cache.Get(cacheKey, acquirer.Acquire<Person>);

            Assert.ReferenceEquals(person1, person2);
        }

        [TestMethod]
        public async Task GetFromCacheSyncAndAsyncShouldReturnSameObject()
        {
            var acquirerMock = CreateAcquirerMock();
            var acquirer = acquirerMock.Object;
            var cache = CreateObjectCache();
            string cacheKey = "FindPerson";

            var person1 = cache.Get(cacheKey, acquirer.Acquire<Person>);
            var person2 = await cache.GetAsync(cacheKey, acquirer.AcquireAsync<Person>);

            Assert.ReferenceEquals(person1, person2);
        }

        // TODO: Execute cache get methods concurrently with a long running acquirer method to test cachelock.
    }
}

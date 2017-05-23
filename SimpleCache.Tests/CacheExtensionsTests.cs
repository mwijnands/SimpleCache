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

        private Mock<PersonAcquirer> CreatePersonAcquirerMock()
        {
            var acquirerMock = new Mock<PersonAcquirer>();

            acquirerMock
                .Setup(a => a.Acquire<Person>())
                .CallBase();

            acquirerMock
                .Setup(a => a.AcquireAsync<Person>())
                .CallBase();

            acquirerMock
                .Setup(a => a.LongRunningAcquireAsync<Person>())
                .CallBase();

            return acquirerMock;
        }

        private Mock<NullPersonAcquirer> CreateNullPersonAcquirerMock()
        {
            var acquirerMock = new Mock<NullPersonAcquirer>();

            acquirerMock
                .Setup(a => a.Acquire<Person>())
                .CallBase();

            acquirerMock
                .Setup(a => a.AcquireAsync<Person>())
                .CallBase();

            acquirerMock
                .Setup(a => a.LongRunningAcquireAsync<Person>())
                .CallBase();

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
            var acquirerMock = CreatePersonAcquirerMock();
            var acquirer = acquirerMock.Object;
            var cache = CreateObjectCache();
            string cacheKey = "FindPerson";

            cache.Get(cacheKey, acquirer.Acquire<Person>);
            cache.Get(cacheKey, acquirer.Acquire<Person>);

            acquirerMock.Verify(a => a.Acquire<Person>(), Times.Once);
        }

        [TestMethod]
        public async Task GetFromCacheSyncAndAsyncShouldCallAcquireOnceAndAcquireAsyncNever()
        {
            var acquirerMock = CreatePersonAcquirerMock();
            var acquirer = acquirerMock.Object;
            var cache = CreateObjectCache();
            string cacheKey = "FindPerson";

            cache.Get(cacheKey, acquirer.Acquire<Person>);
            await cache.GetAsync(cacheKey, acquirer.AcquireAsync<Person>);

            acquirerMock.Verify(a => a.Acquire<Person>(), Times.Once);
            acquirerMock.Verify(a => a.AcquireAsync<Person>(), Times.Never);
        }

        [TestMethod]
        public void GetFromCacheTwiceShouldReturnSameObject()
        {
            var acquirerMock = CreatePersonAcquirerMock();
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
            var acquirerMock = CreatePersonAcquirerMock();
            var acquirer = acquirerMock.Object;
            var cache = CreateObjectCache();
            string cacheKey = "FindPerson";

            var person1 = cache.Get(cacheKey, acquirer.Acquire<Person>);
            var person2 = await cache.GetAsync(cacheKey, acquirer.AcquireAsync<Person>);

            Assert.ReferenceEquals(person1, person2);
        }

        [TestMethod]
        public async Task GetFromCacheConcurrentlyShouldCallLongRunningAcquireAsyncOnlyOnce()
        {
            var acquirerMock = CreatePersonAcquirerMock();
            var acquirer = acquirerMock.Object;
            var cache = CreateObjectCache();
            string cacheKey = "FindPerson";

            var person1Task = cache.GetAsync(cacheKey, acquirer.LongRunningAcquireAsync<Person>);
            var person2Task = cache.GetAsync(cacheKey, acquirer.LongRunningAcquireAsync<Person>);
            var person3Task = cache.GetAsync(cacheKey, acquirer.LongRunningAcquireAsync<Person>);
            var person4Task = cache.GetAsync(cacheKey, acquirer.LongRunningAcquireAsync<Person>);
            var person5Task = cache.GetAsync(cacheKey, acquirer.LongRunningAcquireAsync<Person>);

            await Task.WhenAll(person1Task, person2Task, person3Task, person4Task, person5Task);

            acquirerMock.Verify(a => a.LongRunningAcquireAsync<Person>(), Times.Once);
        }

        [TestMethod]
        public void GetFromCacheTwiceShouldCallAcquireOnlyOnceWhenResultIsNull()
        {
            var acquirerMock = CreateNullPersonAcquirerMock();
            var acquirer = acquirerMock.Object;
            var cache = CreateObjectCache();
            string cacheKey = "FindPerson";

            cache.Get(cacheKey, acquirer.Acquire<Person>);
            cache.Get(cacheKey, acquirer.Acquire<Person>);

            acquirerMock.Verify(a => a.Acquire<Person>(), Times.Once);
        }

        [TestMethod]
        public void GetFromCacheShouldReturnMullWhenResultIsNull()
        {
            var acquirerMock = CreateNullPersonAcquirerMock();
            var acquirer = acquirerMock.Object;
            var cache = CreateObjectCache();
            string cacheKey = "FindPerson";

            var person = cache.Get(cacheKey, acquirer.Acquire<Person>);

            Assert.IsNull(person);
        }

        // TODO: Add tests for expirationdate, expirationperiod and filemonitoring.
        //       Also add tests for removing cacheentries and clearing cache for specific types.
    }
}

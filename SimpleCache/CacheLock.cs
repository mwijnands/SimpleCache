using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace XperiCode.SimpleCache
{
    public class CacheLock : IDisposable
    {
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks;
        private readonly SemaphoreSlim _lock;

        static CacheLock()
        {
            _locks = new ConcurrentDictionary<string, SemaphoreSlim>();
        }

        public static CacheLock Lock(string key, string regionName)
        {
            var myLock = Create(key, regionName);
            myLock.Wait();
            return myLock;
        }

        public async static Task<CacheLock> LockAsync(string key, string regionName)
        {
            var myLock = Create(key, regionName);
            await myLock.WaitAsync();
            return myLock;
        }

        private CacheLock(string uniqueKey)
        {
            _locks.TryAdd(uniqueKey, new SemaphoreSlim(1, 1));
            _lock = _locks[uniqueKey];
        }
  
        private static CacheLock Create(string key, string regionName)
        {
            var uniqueKey = GenerateUniqueKey(key, regionName);
            var myLock = new CacheLock(uniqueKey);
            return myLock;
        }
  
        private static string GenerateUniqueKey(string key, string regionName)
        {
            return string.Format("[{0}][{1}]", regionName, key);
        }

        private void Wait()
        {
            _lock.Wait();
        }

        private async Task WaitAsync()
        {
            await _lock.WaitAsync();
        }

        #region IDisposable Members

        private Boolean _disposed;

        private void Dispose(Boolean disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_lock != null)
                {
                    _lock.Release();

                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

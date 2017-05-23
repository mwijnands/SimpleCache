using System;
using System.Linq;
using System.Threading.Tasks;

namespace XperiCode.SimpleCache.Tests.Test
{
    public abstract class NullPersonAcquirer
    {
        private readonly Person _person;

        public NullPersonAcquirer()
        {
            _person = null;
        }

        public virtual Person Acquire<T>()
        {
            return _person;
        }

        public virtual Task<Person> AcquireAsync<T>()
        {
            return Task.FromResult(_person);
        }

        public virtual async Task<Person> LongRunningAcquireAsync<T>()
        {
            await Task.Delay(500);
            return _person;
        }
    }
}

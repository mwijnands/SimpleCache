using System;
using System.Linq;
using System.Threading.Tasks;

namespace XperiCode.SimpleCache.Tests.Test
{
    public abstract class PersonAcquirer
    {
        private readonly Person _person;

        public PersonAcquirer()
        {
            _person = new Person
            { 
                Name = "John Doe", 
                Age = 35 
            };
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

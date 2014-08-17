using System;
using System.Linq;
using System.Threading.Tasks;

namespace XperiCode.SimpleCache.Tests.Test
{
    public interface IAcquirer
    {
        T Acquire<T>();
        Task<T> AcquireAsync<T>();
    }
}

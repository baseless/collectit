using System;

namespace CollectIt.Web.Interfaces
{
    public interface IManagerFactory<out T> : IDisposable
    {
        T Manager { get; }
    }
}

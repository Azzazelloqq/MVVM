using System;

namespace MVVM.MVVM.ReactiveLibrary.Base
{
public interface IReactive : IDisposable
{
    public bool IsDisposed { get; }
}
}
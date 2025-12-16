#if !PROJECT_SUPPORT_R3
using System;

namespace Azzazelloqq.MVVM.ReactiveLibrary
{
/// <summary>
/// A base interface for reactive objects, providing a mechanism for releasing resources
/// and tracking the disposal state.
/// </summary>
public interface IReactive : IDisposable
{
    /// <summary>
    /// Indicates whether the object has been disposed.
    /// </summary>
    /// <value>
    /// <c>true</c> if the object has been disposed; otherwise, <c>false</c>.
    /// </value>
    public bool IsDisposed { get; }
}
}
#endif

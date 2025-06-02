using System;
using System.Collections.Generic;

namespace Azzazelloqq.MVVM.Source.ReactiveLibrary.Notifier
{
/// <summary>
/// Represents a reactive notifier that allows for subscription to notifications and triggers those notifications
/// when the <see cref="Notify"/> method is called. Implements <see cref="IReactiveNotifier"/>.
/// </summary>
public class ReactiveNotifier : IReactiveNotifier
{
    /// <inheritdoc/>
    public bool IsDisposed { get; private set; }

    private readonly List<Action> _listeners;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveNotifier"/> class with a specified capacity for listeners.
    /// </summary>
    /// <param name="listenersCapacity">The initial capacity for the listener list.</param>
    public ReactiveNotifier(int listenersCapacity = 30)
    {
        _listeners = new List<Action>(listenersCapacity);
    }
    
    /// <inheritdoc/>
    public void Notify()
    {
        if (IsDisposed)
        {
            return;
        }

        foreach (var listener in _listeners)
        {
            listener.Invoke();
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _listeners.Clear();
        
        IsDisposed = true;
    }

    /// <inheritdoc/>
    public void Subscribe(Action onNotify)
    {
        _listeners.Add(onNotify);
    }

    /// <inheritdoc/>
    public void Unsubscribe(Action onNotify)
    {
        _listeners.Remove(onNotify);
    }
}
}
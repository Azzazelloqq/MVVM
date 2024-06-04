using System;
using System.Collections.Generic;

namespace MVVM.MVVM.ReactiveLibrary.Notifier
{
public class ReactiveNotifier : IReactiveNotifier
{
    public bool IsDisposed { get; private set; }

    private readonly List<Action> _listeners;
    
    public ReactiveNotifier(int listenersCapacity = 30)
    {
        _listeners = new List<Action>(listenersCapacity);
    }
    
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

    public void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _listeners.Clear();
        
        IsDisposed = true;
    }

    public void Subscribe(Action onNotify)
    {
        _listeners.Add(onNotify);
    }

    public void Unsubscribe(Action onNotify)
    {
        _listeners.Remove(onNotify);
    }
}
}
using System;
using System.Collections.Generic;

namespace MVVM.MVVM.ReactiveLibrary.Property
{
public class ReactiveProperty<TValue> : IReactiveProperty<TValue>
{
    public bool IsDisposed { get; private set; }
    public bool HasValue => Value != null;
    public TValue Value { get; private set; }

    private readonly List<Action<TValue>> _callbacks;
    private readonly List<Action<TValue>> _onceCallbacks;

    public ReactiveProperty(int listenersCapacity = 30)
    {
        _callbacks = new List<Action<TValue>>(listenersCapacity);
        _onceCallbacks = new List<Action<TValue>>(listenersCapacity);
    }

    public void SetValue(TValue value)
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(ReactiveProperty<TValue>));
        }

        if (Equals(Value, value))
        {
            return;
        }

        Value = value;
        OnValueChanged(Value);
    }

    public void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }
        
        IsDisposed = true;
        _callbacks.Clear();
    }

    public void Subscribe(Action<TValue> onValueChanged, bool withNotify = true)
    {
        if (withNotify)
        {
            onValueChanged?.Invoke(Value);
        }

        _callbacks.Add(onValueChanged);
    }

    public void SubscribeOnce(Action<TValue> onValueChanged)
    {
        _onceCallbacks.Add(onValueChanged);
    }

    public void Unsubscribe(Action<TValue> onValueChanged)
    {
        _callbacks.Remove(onValueChanged);
    }

    private void OnValueChanged(TValue value)
    {
        foreach (var callback in _callbacks)
        {
            callback?.Invoke(value);
        }

        foreach (var callback in _onceCallbacks)
        {
            callback?.Invoke(value);
        }
        
        _onceCallbacks.Clear();
    }
}
}
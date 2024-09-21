using System;
using System.Collections.Generic;

namespace MVVM.MVVM.ReactiveLibrary.Property
{
/// <summary>
/// Represents a reactive property that supports both reading and setting the value, 
/// and provides notifications when the value changes. Implements <see cref="IReactiveProperty{TValue}"/>.
/// </summary>
/// <typeparam name="TValue">The type of the value stored in the property.</typeparam>
public class ReactiveProperty<TValue> : IReactiveProperty<TValue>
{
    /// <inheritdoc/>
    public bool IsDisposed { get; private set; }
    /// <inheritdoc/>
    public bool HasValue => Value != null;
    /// <inheritdoc/>
    public TValue Value { get; private set; }

    private readonly List<Action<TValue>> _callbacks;
    private readonly List<Action<TValue>> _onceCallbacks;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveProperty{TValue}"/> class without an initial value.
    /// </summary>
    /// <param name="listenersCapacity">The initial capacity for event listeners.</param>
    public ReactiveProperty(int listenersCapacity = 30)
    {
        _callbacks = new List<Action<TValue>>(listenersCapacity);
        _onceCallbacks = new List<Action<TValue>>(listenersCapacity);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveProperty{TValue}"/> class without an initial value.
    /// </summary>
    /// <param name="listenersCapacity">The initial capacity for event listeners.</param>
    public ReactiveProperty(TValue value, int listenersCapacity = 30)
    {
        Value = value;
        
        _callbacks = new List<Action<TValue>>(listenersCapacity);
        _onceCallbacks = new List<Action<TValue>>(listenersCapacity);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }
        
        IsDisposed = true;
        _callbacks.Clear();
    }

    /// <inheritdoc/>
    public void Subscribe(Action<TValue> onValueChanged, bool withNotify = true)
    {
        if (withNotify)
        {
            onValueChanged?.Invoke(Value);
        }

        _callbacks.Add(onValueChanged);
    }

    /// <inheritdoc/>
    public void SubscribeOnce(Action<TValue> onValueChanged)
    {
        _onceCallbacks.Add(onValueChanged);
    }

    /// <inheritdoc/>
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
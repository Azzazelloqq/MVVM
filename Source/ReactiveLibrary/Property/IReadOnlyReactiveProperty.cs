using System;
using Azzazelloqq.MVVM.Source.ReactiveLibrary.Base;
using Azzazelloqq.MVVM.Source.ReactiveLibrary.Callbacks;

namespace Azzazelloqq.MVVM.Source.ReactiveLibrary.Property
{
/// <summary>
/// Represents a read-only reactive property that allows subscribing to value changes.
/// Inherits from <see cref="IReactive"/>.
/// </summary>
/// <typeparam name="TValue">The type of the value stored in the property.</typeparam>
public interface IReadOnlyReactiveProperty<TValue> : IReactive
{
    /// <summary>
    /// Gets the current value of the property.
    /// </summary>
    public TValue Value { get; }

    /// <summary>
    /// Subscribes to be notified when the value of the property changes.
    /// Optionally notifies immediately with the current value.
    /// </summary>
    /// <param name="onValueChanged">The action to invoke when the value changes.</param>
    /// <param name="withNotify">
    /// If <c>true</c>, immediately notifies the subscriber with the current value; otherwise, waits for the next change.
    /// </param>
    public Subscription<TValue> Subscribe(Action<TValue> onValueChanged, bool withNotify = true);
    
    /// <summary>
    /// Subscribes to be notified of the next value change, then automatically unsubscribes after the first notification.
    /// </summary>
    /// <param name="onValueChanged">The action to invoke when the value changes.</param>
    public Subscription<TValue> SubscribeOnce(Action<TValue> onValueChanged);

    /// <summary>
    /// Unsubscribes from value change notifications.
    /// </summary>
    /// <param name="onValueChanged">The action that was previously subscribed.</param>
    public void Unsubscribe(Action<TValue> onValueChanged);
}
}
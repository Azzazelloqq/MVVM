using System;

namespace Azzazelloqq.MVVM.Source.ReactiveLibrary.Callbacks
{
/// <summary>
/// Defines a buffer of callbacks that can be subscribed to, unsubscribed from,
/// and invoked. Supports one-time subscriptions and disposal of all callbacks.
/// </summary>
/// <typeparam name="T">The type of the value passed to callback actions.</typeparam>
internal interface ICallbacks<T> : IDisposable
{
	/// <summary>
	/// Subscribes an action to be executed when the event is triggered.
	/// </summary>
	/// <param name="action">The action to be executed when the event occurs.</param>
	/// <returns>A subscription object that can be used to unsubscribe from the event.</returns>

	public Subscription<T> Subscribe(Action<T> action);
	
	/// <summary>
	/// Subscribes an action to be executed when the event is triggered and immediately
	/// executes the action with the provided value.
	/// </summary>
	/// <param name="action">The action to be executed when the event occurs.</param>
	/// <param name="value">The initial value to pass to the action immediately after subscribing.</param>
	/// <returns>A subscription object that can be used to unsubscribe from the event.</returns>

	public Subscription<T> SubscribeWithNotify(Action<T> action, T value);
	
	/// <summary>
	/// Subscribes to receive a single notification. After the first invocation,
	/// the callback is automatically unsubscribed.
	/// </summary>
	/// <param name="action">The callback to invoke on the next <see cref="Notify"/> call.</param>
	/// <returns>
	/// A <see cref="Subscription{T}"/> token that can be disposed to cancel before the next notification.
	/// </returns>
	public Subscription<T> SubscribeOnce(Action<T> action);
	
	/// <summary>
	/// Unsubscribes a previously registered callback.
	/// If the callback was registered via <see cref="SubscribeOnce"/>, it is removed 
	/// before it has a chance to run; if it has already run, this method has no effect.
	/// </summary>
	/// <param name="action">The callback to remove.</param>
	public void Unsubscribe(Action<T> action);
	
	/// <summary>
	/// Invokes all registered callbacks with the provided <paramref name="value"/>.
	/// One-time callbacks are invoked and then removed.
	/// </summary>
	/// <param name="value">The value to pass to each callback.</param>
	public void Notify(T value);
	
	/// <summary>
	/// Clears all registered callbacks, including one-time subscriptions.
	/// </summary>
	public void Clear();
}
}
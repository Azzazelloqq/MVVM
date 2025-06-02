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
	/// Subscribes to receive notifications indefinitely. If <paramref name="withNotify"/>
	/// is <c>true</c>, the callback is immediately invoked with <paramref name="initialValue"/>.
	/// </summary>
	/// <param name="action">The callback to invoke when <see cref="Notify"/> is called.</param>
	/// <param name="withNotify">
	/// If <c>true</c>, invokes <paramref name="action"/> immediately with <paramref name="initialValue"/>.
	/// </param>
	/// <param name="initialValue">
	/// The value to send to <paramref name="action"/> immediately when <paramref name="withNotify"/> is <c>true</c>.
	/// </param>
	/// <returns>
	/// A <see cref="Subscription{T}"/> token that can be disposed to unsubscribe.
	/// </returns>
	public Subscription<T> Subscribe(Action<T> action, bool withNotify = false, T initialValue = default);
	
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
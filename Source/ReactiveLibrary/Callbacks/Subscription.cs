using System;

namespace Azzazelloqq.MVVM.Source.ReactiveLibrary.Callbacks
{
/// <summary>
/// Token returned by <see cref="ICallbacks{T}.Subscribe"/> for disposing the subscription without heap allocation.
/// </summary>
/// <typeparam name="TValue">The type of the value passed to callbacks.</typeparam>
public struct Subscription<TValue> : IDisposable
{
	private readonly ICallbacks<TValue> _owner;
	private readonly Action<TValue> _callback;
	private bool _disposed;

	/// <summary>
	/// Initializes a new instance of the <see cref="Subscription{TValue}"/> struct.
	/// </summary>
	/// <param name="owner">
	/// The <see cref="ICallbacks{TValue}"/> instance that owns this subscription.
	/// </param>
	/// <param name="callback">The callback action registered for notifications.</param>
	internal Subscription(ICallbacks<TValue> owner, Action<TValue> callback)
	{
		_owner = owner;
		_callback = callback;
		_disposed = false;
	}

	/// <summary>
	/// Disposes the subscription by calling <see cref="ICallbacks{TValue}.Unsubscribe"/>.
	/// If already disposed, this method has no effect.
	/// </summary>
	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}

		_disposed = true;
		_owner.Unsubscribe(_callback);
	}
}
}
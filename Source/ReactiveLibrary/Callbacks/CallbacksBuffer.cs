#if !PROJECT_SUPPORT_R3
using System;
using System.Collections.Generic;

namespace Azzazelloqq.MVVM.ReactiveLibrary.Callbacks
{
/// <summary>
/// Implements <see cref="ICallbacks{T}"/> by maintaining a list of callback actions,
/// supporting immediate or deferred notification, one-time subscriptions, and
/// versioned caching for efficient invocation without extra allocations.
/// </summary>
/// <typeparam name="T">The type of the value passed to callback actions.</typeparam>
internal class CallbackBuffer<T> : ICallbacks<T>
{
	private readonly object _lock = new();
	private readonly int _callbacksCapacity;

	private List<Action<T>> _callbacks;
	private List<Action<T>> _onceCallbacks;
	private Action<T>[] _cache = Array.Empty<Action<T>>();

	private int _cacheCount;
	private int _cacheVersion;
	private int _cachedVersion = -1;

	public CallbackBuffer(int capacity = 30)
	{
		_callbacksCapacity = capacity;
	}

	/// <inheritdoc/>
	public Subscription<T> Subscribe(Action<T> action)
	{
		return Subscribe(action, false);
	}

	/// <inheritdoc/>
	public Subscription<T> SubscribeWithNotify(Action<T> action, T value)
	{
		return Subscribe(action, true, value);
	}

	/// <inheritdoc/>
	public Subscription<T> SubscribeOnce(Action<T> action)
	{
		if (action == null)
		{
			throw new ArgumentNullException(nameof(action));
		}

		lock (_lock)
		{
			_onceCallbacks ??= new List<Action<T>>(_callbacksCapacity);
			_onceCallbacks.Add(action);
		}

		return new Subscription<T>(this, action);
	}

	/// <inheritdoc/>
	public void Unsubscribe(Action<T> action)
	{
		if (action == null)
		{
			return;
		}

		lock (_lock)
		{
			var isCallbacksCountChanged = false;

			if (_callbacks != null)
			{
				_callbacks.Remove(action);
				isCallbacksCountChanged = true;
			}

			if (isCallbacksCountChanged)
			{
				_cacheVersion++;
			}

			_onceCallbacks?.Remove(action);
		}
	}

	/// <inheritdoc/>
	public void Clear()
	{
		ClearAllData();
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		ClearAllData();
	}

	/// <inheritdoc/>
	public void Notify(T value)
	{
		NotifyCallbacks(value);
		NotifyOnceCallbacks(value);
	}

	/// <summary>
	/// Invokes and caches the permanent callbacks if the underlying list has changed.
	/// </summary>
	/// <param name="value">The value to pass to each registered callback.</param>
	private void NotifyCallbacks(T value)
	{
		if (_callbacks == null)
		{
			return;
		}

		Action<T>[] buffer;
		int count;

		lock (_lock)
		{
			if (_cacheVersion != _cachedVersion)
			{
				count = _callbacks.Count;

				if (_cache.Length > count * 2 || _cache.Length < count)
				{
					_cache = new Action<T>[count];
				}

				_callbacks.CopyTo(_cache, 0);
				_cacheCount = count;
				_cachedVersion = _cacheVersion;
			}
			else
			{
				count = _cacheCount;
			}

			buffer = _cache;
		}

		for (var i = 0; i < count; i++)
		{
			buffer[i](value);
		}
	}

	/// <summary>
	/// Invokes and then clears all one-time callbacks.
	/// </summary>
	/// <param name="value">The value to pass to each one-time callback.</param>
	private void NotifyOnceCallbacks(T value)
	{
		if (_onceCallbacks == null)
		{
			return;
		}

		Action<T>[] onceCopy;
		int onceCount;
		lock (_lock)
		{
			onceCount = _onceCallbacks.Count;
			onceCopy = onceCount > 0
				? _onceCallbacks.ToArray()
				: Array.Empty<Action<T>>();

			_onceCallbacks.Clear();
		}

		for (var i = 0; i < onceCount; i++)
		{
			onceCopy[i](value);
		}
	}
	
	/// <summary>
	/// Internal implementation of the subscription mechanism.
	/// Subscribes an action to be executed when the event is triggered.
	/// </summary>
	/// <param name="action">The action to be executed when the event occurs.</param>
	/// <param name="withNotify">If true, immediately executes the action with the provided value.</param>
	/// <param name="value">The value to pass to the action if withNotify is true. Default value is used if not specified.</param>
	/// <returns>A subscription object that can be used to unsubscribe from the event.</returns>
	/// <exception cref="ArgumentNullException">Thrown when action is null.</exception>
	private Subscription<T> Subscribe(Action<T> action, bool withNotify, T value = default)
	{
		if (action == null)
		{
			throw new ArgumentNullException(nameof(action));
		}

		lock (_lock)
		{
			_callbacks ??= new List<Action<T>>(_callbacksCapacity);

			_callbacks.Add(action);
			_cacheVersion++;
		}

		if (withNotify)
		{
			action(value);
		}

		return new Subscription<T>(this, action);
	}

	private void ClearAllData()
	{
		lock (_lock)
		{
			_callbacks?.Clear();
			_onceCallbacks?.Clear();
			_cacheCount = 0;
			_cacheVersion++;
			_cachedVersion = -1;
		}
	}
}
}
#endif

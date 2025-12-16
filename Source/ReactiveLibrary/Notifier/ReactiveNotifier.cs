#if !PROJECT_SUPPORT_R3
using System;
using System.Collections.Generic;

namespace Azzazelloqq.MVVM.ReactiveLibrary
{
/// <summary>
/// Represents a reactive notifier that allows for subscription to notifications and triggers those notifications
/// when the <see cref="Notify"/> method is called. Implements <see cref="IReactiveNotifier"/>.
/// </summary>
public class ReactiveNotifier : IReactiveNotifier
{
	public bool IsDisposed { get; private set; }

	private readonly object _lock = new();
	private readonly int _capacity;
	private List<Action> _callbacks;
	private Action[] _cache = Array.Empty<Action>(); 

	private int _cacheVersion;
	private int _cachedVersion = -1; 
	private int _cacheCount; 

	public ReactiveNotifier(int listenersCapacity = 30)
	{
		_capacity = listenersCapacity;
	}

	public void Subscribe(Action onNotify)
	{
		if (onNotify == null)
		{
			throw new ArgumentNullException(nameof(onNotify));
		}

		ThrowIfDisposed();

		lock (_lock)
		{
			_callbacks ??= new List<Action>(_capacity);
			_callbacks.Add(onNotify);
			_cacheVersion++;
		}
	}

	public void Unsubscribe(Action onNotify)
	{
		if (onNotify == null)
		{
			return;
		}

		lock (_lock)
		{
			if (_callbacks != null && _callbacks.Remove(onNotify))
			{
				_cacheVersion++;
			}
		}
	}

	public void Notify()
	{
		if (IsDisposed)
		{
			return;
		}

		if (_callbacks == null)
		{
			return; // ��� ����� �� ������������
		}

		Action[] local;
		int count;

		lock (_lock)
		{
			if (_cacheVersion != _cachedVersion)
			{
				count = _callbacks.Count;

				if (_cache.Length > count * 2 || _cache.Length < count)
				{
					_cache = new Action[count];
				}

				_callbacks.CopyTo(_cache, 0);
				_cacheCount = count;
				_cachedVersion = _cacheVersion;
			}
			else
			{
				count = _cacheCount;
			}

			local = _cache; 
		}

		for (var i = 0; i < count; i++)
		{
			local[i]?.Invoke();
		}
	}

	public void Dispose()
	{
		if (IsDisposed)
		{
			return;
		}

		lock (_lock)
		{
			_callbacks?.Clear();
			_cacheCount = 0;
			_cacheVersion++;
			_cachedVersion = -1;
		}

		IsDisposed = true;
	}

	private void ThrowIfDisposed()
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(nameof(ReactiveNotifier));
		}
	}
}
}
#endif
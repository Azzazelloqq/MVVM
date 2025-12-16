#if !PROJECT_SUPPORT_R3
using System;
using System.Collections.Generic;
using Azzazelloqq.MVVM.ReactiveLibrary.Callbacks;

namespace Azzazelloqq.MVVM.ReactiveLibrary
{
	/// <summary>
	/// Represents a reactive property that supports both reading and setting the value,
	/// and provides notifications when the value changes.
	/// </summary>
	/// <typeparam name="TValue">The type of the value stored in the property.</typeparam>
	public class ReactiveProperty<TValue> : IReactiveProperty<TValue>
	{
		/// <inheritdoc/>
		public bool IsDisposed { get; private set; }

		/// <inheritdoc/>
		public TValue Value { get; private set; }

		private readonly ICallbacks<TValue> _callbacks;

		/// <summary>
		/// Initializes a new instance without an initial value.
		/// </summary>
		public ReactiveProperty(int listenersCapacity = 30)
		{
			_callbacks = new CallbackBuffer<TValue>(listenersCapacity);
		}

		/// <summary>
		/// Initializes a new instance with an initial value.
		/// </summary>
		public ReactiveProperty(TValue initial, int listenersCapacity = 30) : this(listenersCapacity)
		{
			Value = initial;
		}

		/// <inheritdoc/>
		public void SetValue(TValue value)
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(nameof(ReactiveProperty<TValue>));
			}

			if (EqualityComparer<TValue>.Default.Equals(Value, value))
			{
				return;
			}

			Value = value;
			_callbacks.Notify(Value);
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}

			_callbacks.Dispose();

			IsDisposed = true;
		}

		/// <inheritdoc/>
		public Subscription<TValue> Subscribe(Action<TValue> onValueChanged, bool withNotify = true)
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(nameof(ReactiveProperty<TValue>));
			}

			if (onValueChanged == null)
			{
				throw new ArgumentNullException(nameof(onValueChanged));
			}

			if (withNotify)
			{
				return _callbacks.SubscribeWithNotify(onValueChanged, Value);
			}

			return _callbacks.Subscribe(onValueChanged);
		}

		/// <inheritdoc/>
		public Subscription<TValue> SubscribeOnce(Action<TValue> onValueChanged)
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(nameof(ReactiveProperty<TValue>));
			}

			if (onValueChanged == null)
			{
				throw new ArgumentNullException(nameof(onValueChanged));
			}

			return _callbacks.SubscribeOnce(onValueChanged);
		}

		/// <inheritdoc/>
		public void Unsubscribe(Action<TValue> onValueChanged)
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(nameof(ReactiveProperty<TValue>));
			}

			_callbacks.Unsubscribe(onValueChanged);
		}
	}
}
#endif

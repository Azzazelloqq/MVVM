using System;
using System.Collections.Generic;
using Azzazelloqq.MVVM.ReactiveLibrary.Callbacks;

namespace Azzazelloqq.MVVM.ReactiveLibrary.Collections
{
//todo:It's worth considering using IReadOnly collections instead of IEnumerable.
/// <summary>
/// Represents a reactive collection that supports read-only access and reactive event notifications for changes.
/// Inherits from <see cref="IReactive"/>.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
public interface IReadOnlyReactiveCollection<T> : IEnumerable<T>, IReactive
{
	/// <summary>
	/// Subscribes to notifications when an item is added to the collection.
	/// </summary>
	/// <param name="onItemAdded">An action to be invoked when an item is added.</param>
	/// <returns>A value-type subscription token.</returns>
	public Subscription<T> SubscribeOnItemAdded(Action<T> onItemAdded);

	/// <summary>
	/// Subscribes to notifications when an item is removed from the collection.
	/// </summary>
	/// <param name="onItemRemoved">An action to be invoked when an item is removed.</param>
	/// <returns>A value-type subscription token.</returns>
	public Subscription<T> SubscribeOnItemRemoved(Action<T> onItemRemoved);

	/// <summary>
	/// Subscribes to notifications when an item is added or removed from the collection.
	/// </summary>
	/// <param name="onItemAdded">An action to be invoked when an item is added.</param>
	/// <param name="onItemRemoved">An action to be invoked when an item is removed.</param>
	/// <returns>
	/// A combined value-type subscription token that removes both subscriptions when disposed.
	/// </returns>
	public CombinedDisposable<Subscription<T>, Subscription<T>> SubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved);

	/// <summary>
	/// Subscribes to notifications when the entire collection changes.
	/// </summary>
	/// <param name="collectionChanged">An action to be invoked when the collection changes.</param>
	/// <param name="notifyOnSubscribe">Indicates whether to notify the subscriber immediately upon subscription.</param>
	/// <returns>A value-type subscription token.</returns>
	public Subscription<IEnumerable<T>> SubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged, bool notifyOnSubscribe = true);

	/// <summary>
	/// Unsubscribes from notifications for when the entire collection changes.
	/// </summary>
	/// <param name="collectionChanged">The action that was previously subscribed.</param>
	public void UnsubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged);

	/// <summary>
	/// Unsubscribes from notifications when an item is added or removed from the collection.
	/// </summary>
	/// <param name="onItemAdded">The action that was previously subscribed for item addition.</param>
	/// <param name="onItemRemoved">The action that was previously subscribed for item removal.</param>
	public void UnsubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved);

	/// <summary>
	/// Unsubscribes from notifications when an item is added to the collection.
	/// </summary>
	/// <param name="onItemAdded">The action that was previously subscribed.</param>
	public void UnsubscribeOnItemAdded(Action<T> onItemAdded);

	/// <summary>
	/// Unsubscribes from notifications when an item is removed from the collection.
	/// </summary>
	/// <param name="onItemRemoved">The action that was previously subscribed.</param>
	public void UnsubscribeOnItemRemoved(Action<T> onItemRemoved);
}
}
using System;
using System.Collections.Generic;

namespace Azzazelloqq.MVVM.ReactiveLibrary.Collections
{
//todo:It's worth considering using IReadOnly collections instead of IEnumerable.
/// <summary>
/// Represents a reactive collection that supports read-only access and reactive event notifications for changes.
/// Inherits from <see cref="IReactive"/>.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
public interface IReadOnlyReactiveCollection<out T> : IEnumerable<T>, IReactive
{
	/// <summary>
	/// Subscribes to notifications when an item is added to the collection.
	/// </summary>
	/// <param name="onItemAdded">An action to be invoked when an item is added.</param>
	public void SubscribeOnItemAdded(Action<T> onItemAdded);

	/// <summary>
	/// Subscribes to notifications when an item is removed from the collection.
	/// </summary>
	/// <param name="onItemRemoved">An action to be invoked when an item is removed.</param>
	public void SubscribeOnItemRemoved(Action<T> onItemRemoved);

	/// <summary>
	/// Subscribes to notifications when an item is added or removed from the collection.
	/// </summary>
	/// <param name="onItemAdded">An action to be invoked when an item is added.</param>
	/// <param name="onItemRemoved">An action to be invoked when an item is removed.</param>
	public void SubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved);

	/// <summary>
	/// Subscribes to notifications when the entire collection changes.
	/// </summary>
	/// <param name="collectionChanged">An action to be invoked when the collection changes.</param>
	/// <param name="notifyOnSubscribe">Indicates whether to notify the subscriber immediately upon subscription.</param>
	public void SubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged, bool notifyOnSubscribe = true);

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
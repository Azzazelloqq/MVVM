using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Azzazelloqq.MVVM.ReactiveLibrary.Collections
{
/// <summary>
/// Represents a reactive dictionary that supports event notifications for changes in the dictionary
/// and provides additional methods for working with key-value pairs.
/// Inherits from <see cref="IReactiveCollection{T}"/> and <see cref="IDictionary{TKey,TValue}"/>.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
public interface IReactiveDictionary<TKey, TValue> :
	IReactiveCollection<KeyValuePair<TKey, TValue>>,
	IDictionary<TKey, TValue>
{
	/// <summary>
	/// Attempts to add the specified key and value to the dictionary.
	/// </summary>
	/// <param name="key">The key to add.</param>
	/// <param name="value">The value to add.</param>
	/// <returns>
	/// <c>true</c> if the key and value were added successfully; 
	/// otherwise, <c>false</c> if the key already exists.
	/// </returns>
	public bool TryAdd(TKey key, TValue value);

	/// <summary>
	/// Reduces the excess space that was allocated for the dictionary, if necessary.
	/// </summary>
	public void TrimExcess();

	/// <summary>
	/// Reduces the excess space for the dictionary to the specified capacity, if necessary.
	/// </summary>
	/// <param name="capacity">The desired capacity.</param>
	public void TrimExcess(int capacity);

	/// <summary>
	/// Determines whether the dictionary contains a specific value.
	/// </summary>
	/// <param name="value">The value to locate in the dictionary.</param>
	/// <returns><c>true</c> if the value is found; otherwise, <c>false</c>.</returns>
	public bool ContainsValue(TValue value);

	/// <summary>
	/// Ensures that the dictionary can hold up to the specified number of entries without resizing.
	/// </summary>
	/// <param name="capacity">The minimum capacity to ensure.</param>
	/// <returns>The current capacity of the dictionary after the operation.</returns>
	public int EnsureCapacity(int capacity);

	/// <summary>
	/// Implements the <see cref="ISerializable"/> interface and returns the data needed to serialize the dictionary.
	/// </summary>
	/// <param name="info">A <see cref="SerializationInfo"/> object containing the information required to serialize the dictionary.</param>
	/// <param name="context">A <see cref="StreamingContext"/> object containing the source and destination of the serialized stream.</param>
	public void GetObjectDataGetObjectData(SerializationInfo info, StreamingContext context);

	/// <summary>
	/// Implements the <see cref="IDeserializationCallback"/> interface and is called when deserialization is complete.
	/// </summary>
	/// <param name="sender">The object that initiated the callback.</param>
	public void OnDeserialization(object sender);

	/// <summary>
	/// Subscribes to notifications when the value for a specific key in the dictionary changes.
	/// </summary>
	/// <param name="action">The action to invoke when the value changes for a specific key.</param>
	public void SubscribeOnValueChangedByKey(Action<KeyValuePair<TKey, TValue>> action);

	/// <summary>
	/// Unsubscribes from notifications when the value for a specific key in the dictionary changes.
	/// </summary>
	/// <param name="action">The action to remove from the subscription.</param>
	public void UnsubscribeOnValueChangedByKey(Action<KeyValuePair<TKey, TValue>> action);
}
}
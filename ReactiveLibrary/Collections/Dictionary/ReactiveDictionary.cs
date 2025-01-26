using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MVVM.MVVM.ReactiveLibrary.Collections.Dictionary
{
/// <summary>
/// A reactive dictionary that provides notifications for additions, removals, and updates of key-value pairs.
/// Implements <see cref="IReactiveDictionary{TKey, TValue}"/>.
/// </summary>
/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
public class ReactiveDictionary<TKey, TValue> : IReactiveDictionary<TKey, TValue>
{
	/// <inheritdoc/>
	public ICollection<TKey> Keys => _dictionary.Keys;

	/// <inheritdoc/>
	public ICollection<TValue> Values => _dictionary.Values;

	/// <inheritdoc/>
	public bool IsDisposed { get; private set; }

	/// <inheritdoc/>
	public int Count => _dictionary.Count;

	/// <inheritdoc/>
	public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).IsReadOnly;

	private List<Action<KeyValuePair<TKey, TValue>>> ItemAddedListeners => _itemAddedListeners??= new List<Action<KeyValuePair<TKey, TValue>>>(_listenersCapacity);
	private List<Action<KeyValuePair<TKey, TValue>>> ItemRemovedListeners => _itemRemovedListeners??= new List<Action<KeyValuePair<TKey, TValue>>>(_listenersCapacity);
	private List<Action<IEnumerable<KeyValuePair<TKey, TValue>>>> CollectionChangedListeners => _collectionChangedListeners??= new List<Action<IEnumerable<KeyValuePair<TKey, TValue>>>>(_listenersCapacity);
	private List<Action<KeyValuePair<TKey, TValue>>> ValueChangedByKeyListeners => _valueChangedByKeyListeners??= new List<Action<KeyValuePair<TKey, TValue>>>(_listenersCapacity);
	
	private List<Action<KeyValuePair<TKey, TValue>>> _itemAddedListeners;
	private List<Action<KeyValuePair<TKey, TValue>>> _itemRemovedListeners;
	private List<Action<IEnumerable<KeyValuePair<TKey, TValue>>>> _collectionChangedListeners;
	private List<Action<KeyValuePair<TKey, TValue>>> _valueChangedByKeyListeners;
	private readonly Dictionary<TKey, TValue> _dictionary;
	private readonly int _listenersCapacity;

	/// <summary>
	/// Initializes a new instance of the <see cref="ReactiveDictionary{TKey, TValue}"/> class with the specified capacity.
	/// </summary>
	/// <param name="capacity">The initial capacity of the dictionary.</param>
	/// <param name="listenersCapacity">The initial capacity for event listeners.</param>
	public ReactiveDictionary(int capacity, int listenersCapacity = 15)
	{
		_dictionary = new Dictionary<TKey, TValue>(capacity);
		_listenersCapacity = listenersCapacity;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ReactiveDictionary{TKey, TValue}"/> class with the specified capacity and comparer.
	/// </summary>
	/// <param name="capacity">The initial capacity of the dictionary.</param>
	/// <param name="comparer">The comparer to use for the dictionary keys.</param>
	/// <param name="listenersCapacity">The initial capacity for event listeners.</param>
	public ReactiveDictionary(int capacity, IEqualityComparer<TKey> comparer, int listenersCapacity = 30)
	{
		_dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
		_listenersCapacity = listenersCapacity;
	}

	/// <inheritdoc/>
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return _dictionary.GetEnumerator();
	}

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		if (IsDisposed)
		{
			return;
		}

		IsDisposed = true;
		_dictionary.Clear();
		ItemAddedListeners.Clear();
		ItemRemovedListeners.Clear();
		CollectionChangedListeners.Clear();
		ValueChangedByKeyListeners.Clear();
	}

	/// <inheritdoc/>
	public void SubscribeOnItemAdded(Action<KeyValuePair<TKey, TValue>> onItemAdded)
	{
		ItemAddedListeners.Add(onItemAdded);
	}

	/// <inheritdoc/>
	public void SubscribeOnItemRemoved(Action<KeyValuePair<TKey, TValue>> onItemRemoved)
	{
		ItemRemovedListeners.Add(onItemRemoved);
	}

	/// <inheritdoc/>
	public void SubscribeOnCollectionChanged(Action<KeyValuePair<TKey, TValue>> onItemAdded,
		Action<KeyValuePair<TKey, TValue>> onItemRemoved)
	{
		ItemAddedListeners.Add(onItemAdded);
		ItemRemovedListeners.Add(onItemRemoved);
	}

	/// <inheritdoc/>
	public void SubscribeOnCollectionChanged(Action<IEnumerable<KeyValuePair<TKey, TValue>>> collectionChanged,
		bool notifyOnSubscribe = true)
	{
		CollectionChangedListeners.Add(collectionChanged);

		if (notifyOnSubscribe)
		{
			collectionChanged.Invoke(_dictionary);
		}
	}

	/// <inheritdoc/>
	public void SubscribeOnValueChangedByKey(Action<KeyValuePair<TKey, TValue>> action)
	{
		ValueChangedByKeyListeners.Add(action);
	}

	/// <inheritdoc/>
	public void UnsubscribeOnValueChangedByKey(Action<KeyValuePair<TKey, TValue>> action)
	{
		ValueChangedByKeyListeners.Remove(action);
	}

	/// <inheritdoc/>
	public void UnsubscribeOnCollectionChanged(Action<IEnumerable<KeyValuePair<TKey, TValue>>> collectionChanged)
	{
		CollectionChangedListeners.Remove(collectionChanged);
	}

	/// <inheritdoc/>
	public void UnsubscribeOnCollectionChanged(Action<KeyValuePair<TKey, TValue>> onItemAdded,
		Action<KeyValuePair<TKey, TValue>> onItemRemoved)
	{
		ItemAddedListeners.Remove(onItemAdded);
		ItemRemovedListeners.Remove(onItemRemoved);
	}

	/// <inheritdoc/>
	public void UnsubscribeOnItemAdded(Action<KeyValuePair<TKey, TValue>> onItemAdded)
	{
		ItemAddedListeners.Remove(onItemAdded);
	}

	/// <inheritdoc/>
	public void UnsubscribeOnItemRemoved(Action<KeyValuePair<TKey, TValue>> onItemRemoved)
	{
		ItemRemovedListeners.Remove(onItemRemoved);
	}

	/// <inheritdoc/>
	public void Add(KeyValuePair<TKey, TValue> item)
	{
		_dictionary.Add(item.Key, item.Value);

		NotifyItemAdded(item);
		NotifyCollectionChanged();
	}

	/// <inheritdoc/>
	public void Clear()
	{
		_dictionary.Clear();

		NotifyCollectionChanged();
	}

	/// <inheritdoc/>
	public bool TryAdd(TKey key, TValue value)
	{
		var isSuccess = _dictionary.TryAdd(key, value);

		if (!isSuccess)
		{
			return false;
		}

		NotifyItemAdded(new KeyValuePair<TKey, TValue>(key, value));
		NotifyCollectionChanged();

		return true;
	}

	/// <inheritdoc/>
	public void TrimExcess()
	{
		_dictionary.TrimExcess();
	}

	/// <inheritdoc/>
	public void TrimExcess(int capacity)
	{
		_dictionary.TrimExcess(capacity);
	}

	/// <inheritdoc/>
	public bool ContainsValue(TValue value)
	{
		return _dictionary.ContainsValue(value);
	}

	/// <inheritdoc/>
	public int EnsureCapacity(int capacity)
	{
		return _dictionary.EnsureCapacity(capacity);
	}

	/// <inheritdoc/>
	public void GetObjectDataGetObjectData(SerializationInfo info, StreamingContext context)
	{
		_dictionary.GetObjectData(info, context);
	}

	/// <inheritdoc/>
	public void OnDeserialization(object sender)
	{
		_dictionary.OnDeserialization(sender);
	}

	/// <inheritdoc/>
	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		return _dictionary.Contains(item);
	}

	/// <inheritdoc/>
	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		if (array == null)
		{
			throw new ArgumentNullException(nameof(array));
		}

		if (arrayIndex < 0 || arrayIndex > array.Length)
		{
			throw new ArgumentOutOfRangeException(nameof(arrayIndex));
		}

		if (array.Length - arrayIndex < _dictionary.Count)
		{
			throw new ArgumentException(
				"The number of elements in the source dictionary is greater than the available space from arrayIndex to the end of the destination array.");
		}

		var i = arrayIndex;
		foreach (var kvp in _dictionary)
		{
			array[i++] = kvp;
		}
	}

	/// <inheritdoc/>
	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		var isRemoved = _dictionary.Remove(item.Key);

		if (!isRemoved)
		{
			return false;
		}

		NotifyItemRemoved(item);
		NotifyCollectionChanged();

		return true;
	}

	/// <inheritdoc/>
	public void Add(TKey key, TValue value)
	{
		_dictionary.Add(key, value);

		NotifyItemAdded(new KeyValuePair<TKey, TValue>(key, value));
		NotifyCollectionChanged();
	}

	/// <inheritdoc/>
	public bool ContainsKey(TKey key)
	{
		return _dictionary.ContainsKey(key);
	}

	/// <inheritdoc/>
	public bool Remove(TKey key)
	{
		var isRemoved = _dictionary.Remove(key, out var value);
		if (!isRemoved)
		{
			return false;
		}

		NotifyItemRemoved(new KeyValuePair<TKey, TValue>(key, value));
		NotifyCollectionChanged();

		return true;
	}

	/// <inheritdoc/>
	public bool TryGetValue(TKey key, out TValue value)
	{
		return _dictionary.TryGetValue(key, out value);
	}

	/// <inheritdoc/>
	public TValue this[TKey key]
	{
		get => _dictionary[key];
		set
		{
			var containsKey = _dictionary.ContainsKey(key);
			_dictionary[key] = value;
			var keyValuePair = new KeyValuePair<TKey, TValue>(key, value);

			if (containsKey)
			{
				NotifyValueChangedByKey(keyValuePair);
			}
			else
			{
				NotifyItemAdded(keyValuePair);
				NotifyCollectionChanged();
			}
		}
	}

	/// <summary>
	/// Notifies subscribers that an item has been added to the dictionary.
	/// </summary>
	/// <param name="item">The item that was added.</param>
	private void NotifyItemAdded(KeyValuePair<TKey, TValue> item)
	{
		foreach (var itemAddedListener in ItemAddedListeners)
		{
			itemAddedListener.Invoke(item);
		}
	}

	/// <summary>
	/// Notifies subscribers that an item has been removed from the dictionary.
	/// </summary>
	/// <param name="item">The item that was removed.</param>
	private void NotifyItemRemoved(KeyValuePair<TKey, TValue> item)
	{
		foreach (var itemRemovedListener in ItemRemovedListeners)
		{
			itemRemovedListener.Invoke(item);
		}
	}

	/// <summary>
	/// Notifies subscribers that the entire collection has changed.
	/// </summary>
	private void NotifyCollectionChanged()
	{
		foreach (var collectionChangedListener in CollectionChangedListeners)
		{
			collectionChangedListener.Invoke(_dictionary);
		}
	}

	/// <summary>
	/// Notifies subscribers that the value for a specific key has changed.
	/// </summary>
	/// <param name="item">The key-value pair that was changed.</param>
	private void NotifyValueChangedByKey(KeyValuePair<TKey, TValue> item)
	{
		foreach (var valueChangedByKeyListener in ValueChangedByKeyListeners)
		{
			valueChangedByKeyListener.Invoke(item);
		}
	}
}
}
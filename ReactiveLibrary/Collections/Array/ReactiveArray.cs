using System;
using System.Collections;
using System.Collections.Generic;
using MVVM.MVVM.ReactiveLibrary.Collections.Base;

namespace MVVM.MVVM.ReactiveLibrary.Collections.Array
{
/// <summary>
/// A reactive array class that provides reactive notifications when elements are modified. 
/// Implements <see cref="IReactiveArray{T}"/> and provides functionality to observe changes 
/// to elements in the array and the array itself.
/// </summary>
/// <typeparam name="T">The type of elements stored in the array.</typeparam>
public class ReactiveArray<T> : IReactiveArray<T>
{
	/// <inheritdoc/>
	public int Count => Length;

	/// <inheritdoc/>
	public int Length => _array.Length;

	/// <inheritdoc/>

	public bool IsReadOnly => _array.IsReadOnly;

	/// <inheritdoc/>
	public bool IsDisposed { get; private set; }

	/// <summary>
	/// The underlying array storing the elements.
	/// </summary>
	private T[] _array;

	private List<Action<T, int>> ItemChangedByIndexListeners => _itemChangedByIndexListeners?? new List<Action<T, int>>(_listenersCapacity);
	private List<Action<IEnumerable<T>>> CollectionChangedListeners => _collectionChangedListeners?? new List<Action<IEnumerable<T>>>(_listenersCapacity);

	private readonly List<Action<T, int>> _itemChangedByIndexListeners;
	private List<Action<IEnumerable<T>>> _collectionChangedListeners;
	private readonly int _listenersCapacity;

	/// <summary>
	/// Initializes a new instance of the <see cref="ReactiveArray{T}"/> class with a specified length 
	/// and an optional capacity for listeners.
	/// </summary>
	/// <param name="length">The length of the array.</param>
	/// <param name="listenersCapacity">The initial capacity for listeners. Defaults to 30.</param>
	public ReactiveArray(int length, int listenersCapacity = 10)
	{
		_array = new T[length];
		_listenersCapacity = listenersCapacity;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ReactiveArray{T}"/> class with a specified array 
	/// and an optional capacity for listeners.
	/// </summary>
	/// <param name="array">The array to initialize with.</param>
	/// <param name="listenersCapacity">The initial capacity for listeners. Defaults to 30.</param>
	public ReactiveArray(IReadOnlyList<T> array, int listenersCapacity = 30)
	{
		_array = new T[array.Count];
		for (var i = 0; i < array.Count; i++)
		{
			_array[i] = array[i];
		}
		
		_listenersCapacity = listenersCapacity;
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		if (IsDisposed)
		{
			return;
		}

		_array = global::System.Array.Empty<T>();
		IsDisposed = true;
	}

	/// <inheritdoc/>
	public IEnumerator<T> GetEnumerator()
	{
		foreach (var item in _array)
		{
			yield return item;
		}
	}

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return _array.GetEnumerator();
	}

	/// <inheritdoc/>
	[Obsolete("An array does not support adding an element", true)]
	public void SubscribeOnItemAdded(Action<T> onItemAdded)
	{
		throw new Exception("An array does not support adding an element");
	}

	/// <inheritdoc/>
	[Obsolete("An array does not support removing an element", true)]
	public void SubscribeOnItemRemoved(Action<T> onItemRemoved)
	{
		throw new Exception("An array does not support removing an element");
	}

	/// <inheritdoc/>
	[Obsolete("An array does not support adding and removing an element", true)]
	public void SubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved)
	{
		throw new Exception("An array does not support adding and removing an element");
	}

	/// <inheritdoc/>
	public void SubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged, bool notifyOnSubscribe = true)
	{
		CollectionChangedListeners.Add(collectionChanged);
	}

	/// <inheritdoc/>
	public void SubscribeOnItemChangedByIndex(Action<T, int> onItemChangedByIndex)
	{
		ItemChangedByIndexListeners.Add(onItemChangedByIndex);
	}

	/// <inheritdoc/>
	public void UnsubscribeOnItemChangedByIndex(Action<T, int> onItemChangedByIndex)
	{
		ItemChangedByIndexListeners.Remove(onItemChangedByIndex);
	}

	/// <inheritdoc/>
	public void UnsubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged)
	{
		CollectionChangedListeners.Remove(collectionChanged);
	}

	/// <inheritdoc/>
	[Obsolete("An array does not support adding and removing an element", true)]
	public void UnsubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved)
	{
		throw new Exception("An array does not support adding and removing an element");
	}

	/// <inheritdoc/>
	[Obsolete("An array does not support adding an element", true)]
	public void UnsubscribeOnItemAdded(Action<T> onItemAdded)
	{
		throw new Exception("An array does not support adding an element");
	}

	/// <inheritdoc/>
	[Obsolete("An array does not support removing an element", true)]
	public void UnsubscribeOnItemRemoved(Action<T> onItemRemoved)
	{
		throw new Exception("An array does not support removing an element");
	}

	public T[] Clone()
	{
		return (T[])_array.Clone();
	}

	[Obsolete("An array does not support adding an element", true)]
	public void Add(T item)
	{
		throw new Exception("An array does not support adding an element");
	}

	public void Clear()
	{
		_array = global::System.Array.Empty<T>();
		NotifyCollectionChanged();
	}

	public bool Contains(T item)
	{
		foreach (var element in _array)
		{
			if (element.Equals(item))
			{
				return true;
			}
		}

		return false;
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		_array.CopyTo(array, arrayIndex);
		NotifyCollectionChanged();
	}

	/// <inheritdoc/>
	[Obsolete("An array does not support removing an element", true)]
	public bool Remove(T item)
	{
		throw new Exception("An array does not support removing an element");
	}

	/// <inheritdoc/>
	public int IndexOf(T item)
	{
		for (var i = 0; i < _array.Length; i++)
		{
			if (_array[i].Equals(item))
			{
				return i;
			}
		}

		return -1;
	}

	/// <inheritdoc/>
	[Obsolete("An array does not support adding an element", true)]
	public void Insert(int index, T item)
	{
		throw new Exception("An array does not support adding an element");
	}

	/// <inheritdoc/>
	[Obsolete("An array does not support removing an element", true)]
	public void RemoveAt(int index)
	{
		throw new Exception("An array does not support removing an element");
	}

	/// <inheritdoc/>
	public T this[int index]
	{
		get => _array[index];
		set
		{
			_array[index] = value;
			NotifyItemChangedByIndex(value, index);
			NotifyCollectionChanged();
		}
	}

	/// <inheritdoc/>
	public int BinarySearch(int index, int length, T value)
	{
		return global::System.Array.BinarySearch(_array, index, length, value);
	}

	/// <inheritdoc/>
	public int BinarySearch(int index, int length, T value, IComparer<T> comparer)
	{
		return global::System.Array.BinarySearch(_array, index, length, value, comparer);
	}

	/// <inheritdoc/>
	public int BinarySearch(T value)
	{
		return global::System.Array.BinarySearch(_array, value);
	}

	/// <inheritdoc/>
	public int BinarySearch(T value, IComparer<T> comparer)
	{
		return global::System.Array.BinarySearch(_array, value, comparer);
	}

	/// <inheritdoc/>
	public void ForEach(Action<T> action)
	{
		foreach (var element in _array)
		{
			action(element);
		}
	}

	/// <inheritdoc/>
	public void Sort()
	{
		global::System.Array.Sort(_array);
		NotifyCollectionChanged();
	}

	/// <inheritdoc/>
	public void Sort(IComparer<T> comparer)
	{
		global::System.Array.Sort(_array, comparer);
		NotifyCollectionChanged();
	}

	/// <inheritdoc/>
	public void Sort(Comparison<T> comparison)
	{
		global::System.Array.Sort(_array, comparison);
		NotifyCollectionChanged();
	}

	/// <inheritdoc/>
	public void Sort(int index, int length)
	{
		global::System.Array.Sort(_array, index, length);
		NotifyCollectionChanged();
	}

	/// <inheritdoc/>
	public void Sort(int index, int length, IComparer<T> comparer)
	{
		global::System.Array.Sort(_array, index, length, comparer);
		NotifyCollectionChanged();
	}

	/// <inheritdoc/>
	public void Reverse()
	{
		global::System.Array.Reverse(_array);
		NotifyCollectionChanged();
	}

	/// <inheritdoc/>
	public void Reverse(int index, int length)
	{
		global::System.Array.Reverse(_array, index, length);
		NotifyCollectionChanged();
	}

	/// <inheritdoc/>
	public T FindLast(Predicate<T> match)
	{
		return global::System.Array.FindLast(_array, match);
	}

	/// <inheritdoc/>
	public T Find(Predicate<T> match)
	{
		return global::System.Array.Find(_array, match);
	}

	/// <inheritdoc/>
	public IReadOnlyReactiveCollection<T> AsReadOnly()
	{
		return this;
	}

	/// <inheritdoc/>
	public IReactiveArray<T> CloneAsReactive()
	{
		return new ReactiveArray<T>(_array);
	}

	/// <summary>
	/// Notifies subscribers that an item has changed at a specific index.
	/// </summary>
	/// <param name="item">The changed item.</param>
	/// <param name="index">The index where the item changed.</param>
	private void NotifyItemChangedByIndex(T item, int index)
	{
		foreach (var itemChangedByIndexListener in ItemChangedByIndexListeners)
		{
			itemChangedByIndexListener.Invoke(item, index);
		}
	}

	/// <summary>
	/// Notifies subscribers that the entire collection has changed.
	/// </summary>
	private void NotifyCollectionChanged()
	{
		foreach (var collectionChangedListener in CollectionChangedListeners)
		{
			collectionChangedListener.Invoke(_array);
		}
	}
}
}
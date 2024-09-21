using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MVVM.MVVM.ReactiveLibrary.Collections.Base;

namespace MVVM.MVVM.ReactiveLibrary.Collections.List
{
/// <summary>
/// Represents a reactive list that provides notifications for additions, removals, and changes to elements.
/// Implements <see cref="IReactiveList{T}"/>.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public class ReactiveList<T> : IReactiveList<T>
{
    /// <inheritdoc/>
    public int Count => _list.Count;
    /// <inheritdoc/>
    public bool IsReadOnly => ((ICollection<T>)_list).IsReadOnly;
    /// <inheritdoc/>
    public bool IsDisposed { get; private set; }

    private readonly List<T> _list;
    private readonly List<Action<T>> _listenersItemAdded;
    private readonly List<Action<T>> _listenersItemRemoved;
    private readonly List<Action<T, int>> _listenersItemAddedAtIndex;
    private readonly List<Action<T, int>> _listenersItemRemovedAtIndex;
    private readonly List<Action<T, int>> _listenersItemChangedAtIndex;
    private readonly List<Action<IEnumerable<T>>> _collectionChangedListeners;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveList{T}"/> class with a specified capacity.
    /// </summary>
    /// <param name="capacity">The initial capacity of the list.</param>
    /// <param name="listenersCapacity">The initial capacity for event listeners.</param>
    public ReactiveList(int capacity = 0, int listenersCapacity = 30)
    {
        _list = new List<T>(capacity);
        _listenersItemAdded = new List<Action<T>>(listenersCapacity);
        _listenersItemRemoved = new List<Action<T>>(listenersCapacity);
        _listenersItemAddedAtIndex = new List<Action<T, int>>(listenersCapacity);
        _listenersItemRemovedAtIndex = new List<Action<T, int>>(listenersCapacity);
        _listenersItemChangedAtIndex = new List<Action<T, int>>(listenersCapacity);
        _collectionChangedListeners = new List<Action<IEnumerable<T>>>(listenersCapacity);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveList{T}"/> class with a collection of elements.
    /// </summary>
    /// <param name="collection">The collection of elements to initialize the list with.</param>
    /// <param name="listenersCapacity">The initial capacity for event listeners.</param>
    public ReactiveList(IEnumerable<T> collection, int listenersCapacity = 30)
    {
        _list = new List<T>(collection);
        _listenersItemAdded = new List<Action<T>>(listenersCapacity);
        _listenersItemRemoved = new List<Action<T>>(listenersCapacity);
        _listenersItemAddedAtIndex = new List<Action<T, int>>(listenersCapacity);
        _listenersItemRemovedAtIndex = new List<Action<T, int>>(listenersCapacity);
        _listenersItemChangedAtIndex = new List<Action<T, int>>(listenersCapacity);
        _collectionChangedListeners = new List<Action<IEnumerable<T>>>(listenersCapacity);
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        IsDisposed = true;

        _list.Clear();
        _listenersItemAdded.Clear();
        _listenersItemRemoved.Clear();
        _collectionChangedListeners.Clear();
        _listenersItemAddedAtIndex.Clear();
        _listenersItemRemovedAtIndex.Clear();
        _listenersItemChangedAtIndex.Clear();
    }
    
    /// <inheritdoc/>
    public void SubscribeOnItemAdded(Action<T> onItemAdded)
    {
        _listenersItemAdded.Add(onItemAdded);
    }
    
    /// <inheritdoc/>
    public void SubscribeOnItemRemoved(Action<T> onItemRemoved)
    {
        _listenersItemRemoved.Add(onItemRemoved);
    }
    
    /// <inheritdoc/>
    public void SubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved)
    {
        SubscribeOnItemAdded(onItemAdded);
        SubscribeOnItemRemoved(onItemRemoved);
    }
    
    /// <inheritdoc/>
    public void SubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged, bool notifyOnSubscribe = true)
    {
        if (notifyOnSubscribe)
        {
            collectionChanged(_list);
        }

        _collectionChangedListeners.Add(collectionChanged);
    }
        
    /// <inheritdoc/>
    public void SubscribeOnItemAddedByIndex(Action<T, int> onItemAdded)
    {
        _listenersItemAddedAtIndex.Add(onItemAdded);
    }
    
    /// <inheritdoc/>
    public void SubscribeOnItemRemovedByIndex(Action<T, int> onItemRemoved)
    {
        _listenersItemRemovedAtIndex.Add(onItemRemoved);
    }
    
    /// <inheritdoc/>
    public void SubscribeOnItemChangedByIndex(Action<T, int> onItemChanged)
    {
        _listenersItemChangedAtIndex.Add(onItemChanged);
    }
    
    /// <inheritdoc/>
    public void UnsubscribeOnItemChangedByIndex(Action<T, int> onItemChanged)
    {
        _listenersItemChangedAtIndex.Remove(onItemChanged);
    }
    
    /// <inheritdoc/>
    public void UnsubscribeOnItemAddedByIndex(Action<T, int> onItemAdded)
    {
        _listenersItemAddedAtIndex.Remove(onItemAdded);
    }
    
    /// <inheritdoc/>
    public void UnsubscribeOnItemRemovedByIndex(Action<T, int> onItemRemoved)
    {
        _listenersItemRemovedAtIndex.Remove(onItemRemoved);
    }
    
    /// <inheritdoc/>
    public void UnsubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged)
    {
        _collectionChangedListeners.Remove(collectionChanged);
    }
    
    /// <inheritdoc/>
    public void UnsubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved)
    {
        _listenersItemAdded.Remove(onItemAdded);
        _listenersItemRemoved.Remove(onItemRemoved);
    }
    
    /// <inheritdoc/>
    public void UnsubscribeOnItemAdded(Action<T> onItemAdded)
    {
        _listenersItemAdded.Remove(onItemAdded);
    }
    
    /// <inheritdoc/>
    public void UnsubscribeOnItemRemoved(Action<T> onItemRemoved)
    {
        _listenersItemRemoved.Remove(onItemRemoved);
    }

    /// <inheritdoc/>
    public T FirstOrDefault(Func<T, bool> predicate)
    {
        foreach (var item in _list)
        {
            if (predicate(item))
            {
                return item;
            }
        }

        return default;
    }

    /// <inheritdoc/>
    public int RemoveAll(Predicate<T> predicate)
    {
        int removedCount = 0;

        for (int i = _list.Count - 1; i >= 0; i--) // Идем с конца списка, чтобы избежать проблем с индексами при удалении
        {
            var toRemove = _list[i];
            if (!predicate(toRemove))
            {
                continue;
            }

            _list.RemoveAt(i);

            NotifyItemRemoved(toRemove);
            removedCount++;
        }

        if (removedCount > 0)
        {
            NotifyCollectionChanged();
        }

        return removedCount;
    }

    /// <inheritdoc/>
    public void Remove(Predicate<T> predicate)
    {
        for (var i = 0; i < _list.Count; i++)
        {
            var toRemove = _list[i];

            if (!predicate(toRemove))
            {
                continue;
            }
            
            _list.RemoveAt(i);
            
            NotifyItemRemoved(toRemove);
            break;
        }
    }

    /// <inheritdoc/>
    public void Sort()
    {
        _list.Sort();
        NotifyCollectionChanged();
    }
    
    /// <inheritdoc/>
    public void Sort(IComparer<T> comparer)
    {
        _list.Sort(comparer);
        NotifyCollectionChanged();
    }
    
    /// <inheritdoc/>
    public void Sort(Comparison<T> comparison)
    {
        _list.Sort(comparison);
        NotifyCollectionChanged();
    }
    
    /// <inheritdoc/>
    public void Sort(int index, int count, IComparer<T> comparer)
    {
        _list.Sort(index, count, comparer);
        NotifyCollectionChanged();
    }
    
    /// <inheritdoc/>
    public int IndexOf(T item, int index)
    {
        return _list.IndexOf(item);
    }
    
    /// <inheritdoc/>
    public int IndexOf(T item, int index, int count)
    {
        return _list.IndexOf(item, index, count);
    }
    
    /// <inheritdoc/>
    public int LastIndexOf(T item)
    {
        return _list.LastIndexOf(item);
    }
    
    /// <inheritdoc/>
    public int LastIndexOf(T item, int index)
    {
        return _list.LastIndexOf(item, index);
    }
        
    /// <inheritdoc/>
    public int LastIndexOf(T item, int index, int count)
    {
        return _list.LastIndexOf(item, index, count);
    }
    
    /// <inheritdoc/>
    public void RemoveRange(int index, int count)
    {
        if (index < 0 || count < 0 || index + count > _list.Count)
        {
            throw new ArgumentOutOfRangeException();
        }

        for (var i = 0; i < count; i++)
        {
            var removedItem = _list[index];
            NotifyItemRemoved(removedItem);
        }
        
        // Notify subscribers about the removal of each item before actually removing them.
        // This is done to optimize the event handling by reducing the number of list modifications.
        _list.RemoveRange(index, count);

        NotifyCollectionChanged();
    }
    
    /// <inheritdoc/>
    public void Reverse()
    {
        _list.Reverse();
        NotifyCollectionChanged();
    }
    
    /// <inheritdoc/>
    public void Reverse(int index, int count)
    {
        _list.Reverse(index, count);
        NotifyCollectionChanged();
    }
    
    /// <inheritdoc/>
    public T[] ToArray()
    {
        return _list.ToArray();
    }
    
    /// <inheritdoc/>
    public void ForEach(Action<T> action)
    {
        _list.ForEach(action);
    }
    
    /// <inheritdoc/>
    public bool Exists(Predicate<T> match)
    {
        return _list.Exists(match);
    }
    
    /// <inheritdoc/>
    public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
    {
        return _list.BinarySearch(index, count, item, comparer);
    }
    
    /// <inheritdoc/>
    public int BinarySearch(T item)
    {
        return _list.BinarySearch(item);
    }
    
    /// <inheritdoc/>
    public int BinarySearch(T item, IComparer<T> comparer)
    {
        return _list.BinarySearch(item, comparer);
    }
    
    /// <inheritdoc/>
    public IReadOnlyReactiveCollection<T> AsReadOnly()
    {
        return this;
    }
    
    /// <inheritdoc/>
    public void AddRange(IEnumerable<T> collection)
    {
        _list.AddRange(collection);
        NotifyCollectionChanged();
    }
    
    /// <inheritdoc/>
    public void Add(T item)
    {
        _list.Add(item);
        
        NotifyItemAdded(item);
        NotifyCollectionChanged();
    }
    
    /// <inheritdoc/>
    public void Clear()
    {
        _list.Clear();
        
        NotifyCollectionChanged();
    }
    
    /// <inheritdoc/>
    public bool Contains(T item)
    {
        return _list.Contains(item);
    }
    
    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
    {
        _list.CopyTo(array, arrayIndex);
        
        NotifyCollectionChanged();
    }
    
    /// <inheritdoc/>
    public bool Remove(T item)
    {
        var isRemoved = _list.Remove(item);
        if (!isRemoved)
        {
            return false;
        }
        
        NotifyItemRemoved(item);
        NotifyCollectionChanged();
        return true;
    }
    
    /// <inheritdoc/>
    public int IndexOf(T item)
    {
        return _list.IndexOf(item);
    }
    
    /// <inheritdoc/>
    public void Insert(int index, T item)
    {
        _list.Insert(index, item);
        NotifyItemAddedAtIndex(item, index);
        NotifyItemAdded(item);
        NotifyCollectionChanged();
    }
    
    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        var itemToRemove = _list[index];
        _list.RemoveAt(index);
        
        NotifyItemRemovedAtIndex(itemToRemove, index);
        NotifyItemRemoved(itemToRemove);
        NotifyCollectionChanged();
    }
    
    /// <inheritdoc/>
    public T this[int index]
    {
        get => _list[index];
        set
        {
            _list[index] = value;
            NotifyItemChangedAtIndex(value, index);
        }
    }
    
    private void NotifyItemAddedAtIndex(T item, int index)
    {
        foreach (var action in _listenersItemAddedAtIndex)
        {
            action.Invoke(item, index);
        }
    }
    
    private void NotifyItemRemovedAtIndex(T item, int index)
    {
        foreach (var action in _listenersItemRemovedAtIndex)
        {
            action.Invoke(item, index);
        }
    }
        
    private void NotifyItemAdded(T item)
    {
        foreach (var action in _listenersItemAdded)
        {
            action.Invoke(item);
        }
    }
    
    private void NotifyItemRemoved(T item)
    {
        foreach (var action in _listenersItemRemoved)
        {
            action.Invoke(item);
        }
    }
    
    private void NotifyCollectionChanged()
    {
        foreach (var action in _collectionChangedListeners)
        {
            action.Invoke(_list);
        }
    }
    
    private void NotifyItemChangedAtIndex(T item, int index)
    {
        foreach (var action in _listenersItemChangedAtIndex)
        {
            action.Invoke(item, index);
        }
    }
}
}
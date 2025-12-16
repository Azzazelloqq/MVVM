#if !PROJECT_SUPPORT_R3
using System;
using System.Collections;
using System.Collections.Generic;
using Azzazelloqq.MVVM.ReactiveLibrary.Callbacks;

namespace Azzazelloqq.MVVM.ReactiveLibrary.Collections
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
    private readonly ICallbacks<T> _listenersItemAdded;
    private readonly ICallbacks<T> _listenersItemRemoved;
    private readonly ICallbacks<(T,int)> _listenersItemAddedAtIndex;
    private readonly ICallbacks<(T,int)> _listenersItemRemovedAtIndex;
    private readonly ICallbacks<(T,int)> _listenersItemChangedAtIndex;
    private readonly ICallbacks<IEnumerable<T>> _collectionChangedListeners;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveList{T}"/> class with a specified capacity.
    /// </summary>
    /// <param name="capacity">The initial capacity of the list.</param>
    /// <param name="listenersCapacity">The initial capacity for event listeners.</param>
    public ReactiveList(int capacity = 0, int listenersCapacity = 30)
    {
        _list = new List<T>(capacity);

        _listenersItemAdded = new CallbackBuffer<T>(listenersCapacity);
        _listenersItemRemoved = new CallbackBuffer<T>(listenersCapacity);
        _listenersItemAddedAtIndex = new CallbackBuffer<(T, int)>(listenersCapacity);
        _listenersItemRemovedAtIndex = new CallbackBuffer<(T, int)>(listenersCapacity);
        _listenersItemChangedAtIndex = new CallbackBuffer<(T, int)>(listenersCapacity);
        _collectionChangedListeners = new CallbackBuffer<IEnumerable<T>>(listenersCapacity);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveList{T}"/> class with a collection of elements.
    /// </summary>
    /// <param name="collection">The collection of elements to initialize the list with.</param>
    /// <param name="listenersCapacity">The initial capacity for event listeners.</param>
    public ReactiveList(IEnumerable<T> collection, int listenersCapacity = 30)
    {
        _list = new List<T>(collection);

        _listenersItemAdded = new CallbackBuffer<T>(listenersCapacity);
        _listenersItemRemoved = new CallbackBuffer<T>(listenersCapacity);
        _listenersItemAddedAtIndex = new CallbackBuffer<(T, int)>(listenersCapacity);
        _listenersItemRemovedAtIndex = new CallbackBuffer<(T, int)>(listenersCapacity);
        _listenersItemChangedAtIndex = new CallbackBuffer<(T, int)>(listenersCapacity);
        _collectionChangedListeners = new CallbackBuffer<IEnumerable<T>>(listenersCapacity);
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
        
        _listenersItemAdded.Dispose();
        _listenersItemRemoved.Dispose();
        _collectionChangedListeners.Dispose();
        _listenersItemAddedAtIndex.Dispose();
        _listenersItemRemovedAtIndex.Dispose();
        _listenersItemChangedAtIndex.Dispose();
    }
    
    /// <inheritdoc/>
    public Subscription<T> SubscribeOnItemAdded(Action<T> onItemAdded)
    {
        return _listenersItemAdded.Subscribe(onItemAdded);
    }
    
    /// <inheritdoc/>
    public Subscription<T> SubscribeOnItemRemoved(Action<T> onItemRemoved)
    {
        return _listenersItemRemoved.Subscribe(onItemRemoved);
    }
    
    /// <inheritdoc/>
    public CombinedDisposable<Subscription<T>, Subscription<T>> SubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved)
    {
        var addedSubscription = SubscribeOnItemAdded(onItemAdded);
        var removedSubscription = SubscribeOnItemRemoved(onItemRemoved);

        return CombinedDisposable.Create(addedSubscription, removedSubscription);
    }
    
    /// <inheritdoc/>
    public Subscription<IEnumerable<T>> SubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged, bool notifyOnSubscribe = true)
    {
        return notifyOnSubscribe
            ? _collectionChangedListeners.SubscribeWithNotify(collectionChanged, _list)
            : _collectionChangedListeners.Subscribe(collectionChanged);
    }
        
    /// <inheritdoc/>
    public Subscription<(T, int)> SubscribeOnItemAddedByIndex(Action<(T, int)> onItemAdded)
    {
        return _listenersItemAddedAtIndex.Subscribe(onItemAdded);
    }
    
    /// <inheritdoc/>
    public Subscription<(T, int)> SubscribeOnItemRemovedByIndex(Action<(T, int)> onItemRemoved)
    {
        return _listenersItemRemovedAtIndex.Subscribe(onItemRemoved);
    }
    
    /// <inheritdoc/>
    public Subscription<(T, int)> SubscribeOnItemChangedByIndex(Action<(T, int)> onItemChanged)
    {
        return _listenersItemChangedAtIndex.Subscribe(onItemChanged);
    }
    
    /// <inheritdoc/>
    public void UnsubscribeOnItemChangedByIndex(Action<(T, int)> onItemChanged)
    {
        _listenersItemChangedAtIndex.Unsubscribe(onItemChanged);
    }
    
    /// <inheritdoc/>
    public void UnsubscribeOnItemAddedByIndex(Action<(T, int)> onItemAdded)
    {
        _listenersItemAddedAtIndex.Unsubscribe(onItemAdded);
    }
    
    /// <inheritdoc/>
    public void UnsubscribeOnItemRemovedByIndex(Action<(T, int)> onItemRemoved)
    {
        _listenersItemRemovedAtIndex.Unsubscribe(onItemRemoved);
    }
    
    /// <inheritdoc/>
    public void UnsubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged)
    {
        _collectionChangedListeners.Unsubscribe(collectionChanged);
    }
    
    /// <inheritdoc/>
    public void UnsubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved)
    {
        _listenersItemAdded.Unsubscribe(onItemAdded);
        _listenersItemRemoved.Unsubscribe(onItemRemoved);
    }
    
    /// <inheritdoc/>
    public void UnsubscribeOnItemAdded(Action<T> onItemAdded)
    {
        _listenersItemAdded.Unsubscribe(onItemAdded);
    }
    
    /// <inheritdoc/>
    public void UnsubscribeOnItemRemoved(Action<T> onItemRemoved)
    {
        _listenersItemRemoved.Unsubscribe(onItemRemoved);
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
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(ReactiveList<T>));
        }
        
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
        _listenersItemAddedAtIndex.Notify((item, index));
    }
    
    private void NotifyItemRemovedAtIndex(T item, int index)
    {
        _listenersItemRemovedAtIndex.Notify((item, index));
    }
        
    private void NotifyItemAdded(T item)
    {
        _listenersItemAdded.Notify(item);
    }
    
    private void NotifyItemRemoved(T item)
    {
        _listenersItemRemoved.Notify(item);
    }
    
    private void NotifyCollectionChanged()
    {
        _collectionChangedListeners.Notify(_list);
    }
    
    private void NotifyItemChangedAtIndex(T item, int index)
    {
        _listenersItemChangedAtIndex.Notify((item, index));
    }
}
}
#endif

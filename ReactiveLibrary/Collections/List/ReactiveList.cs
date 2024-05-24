using System;
using System.Collections;
using System.Collections.Generic;

namespace MVVM.MVVM.ReactiveLibrary.Collections.List
{
public class ReactiveList<T> : IReactiveList<T>
{
    public int Count => _list.Count;
    public bool IsReadOnly => ((ICollection<T>)_list).IsReadOnly;
    public bool IsDisposed { get; private set; }

    private readonly List<T> _list;
    private readonly List<Action<T>> _listenersItemAdded;
    private readonly List<Action<T>> _listenersItemRemoved;
    private readonly List<Action<T, int>> _listenersItemAddedAtIndex;
    private readonly List<Action<T, int>> _listenersItemRemovedAtIndex;
    private readonly List<Action<IEnumerable<T>>> _collectionChangedListeners;

    public ReactiveList(int capacity, int listenersCapacity = 30)
    {
        _list = new List<T>(capacity);
        _listenersItemAdded = new List<Action<T>>(listenersCapacity);
        _listenersItemRemoved = new List<Action<T>>(listenersCapacity);
        _listenersItemAddedAtIndex = new List<Action<T, int>>(listenersCapacity);
        _listenersItemRemovedAtIndex = new List<Action<T, int>>(listenersCapacity);
        _collectionChangedListeners = new List<Action<IEnumerable<T>>>(listenersCapacity);
    }

    public ReactiveList(IEnumerable<T> collection, int listenersCapacity = 30)
    {
        _list = new List<T>(collection);
        _listenersItemAdded = new List<Action<T>>(listenersCapacity);
        _listenersItemRemoved = new List<Action<T>>(listenersCapacity);
        _listenersItemAddedAtIndex = new List<Action<T, int>>(listenersCapacity);
        _listenersItemRemovedAtIndex = new List<Action<T, int>>(listenersCapacity);
        _collectionChangedListeners = new List<Action<IEnumerable<T>>>(listenersCapacity);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Dispose()
    {
        IsDisposed = true;

        _list.Clear();
        _listenersItemAdded.Clear();
        _listenersItemRemoved.Clear();
        _collectionChangedListeners.Clear();
        _listenersItemAddedAtIndex.Clear();
        _listenersItemRemovedAtIndex.Clear();
    }

    public void SubscribeOnItemAdded(Action<T> onItemAdded)
    {
        _listenersItemAdded.Add(onItemAdded);
    }

    public void SubscribeOnItemRemoved(Action<T> onItemRemoved)
    {
        _listenersItemRemoved.Add(onItemRemoved);
    }

    public void SubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved)
    {
        SubscribeOnItemAdded(onItemAdded);
        SubscribeOnItemRemoved(onItemRemoved);
    }

    public void SubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged, bool notifyOnSubscribe = true)
    {
        if (notifyOnSubscribe)
        {
            collectionChanged(_list);
        }

        _collectionChangedListeners.Add(collectionChanged);
    }
    
    public void SubscribeOnItemAddedByIndex(Action<T, int> onItemAdded)
    {
        _listenersItemAddedAtIndex.Add(onItemAdded);
    }

    public void SubscribeOnItemRemovedByIndex(Action<T, int> onItemRemoved)
    {
        _listenersItemRemovedAtIndex.Add(onItemRemoved);
    }

    public void UnsubscribeOnItemAddedByIndex(Action<T, int> onItemAdded)
    {
        _listenersItemAddedAtIndex.Remove(onItemAdded);
    }

    public void UnsubscribeOnItemRemovedByIndex(Action<T, int> onItemRemoved)
    {
        _listenersItemRemovedAtIndex.Remove(onItemRemoved);
    }

    public void UnsubscribeOnCollectionChanged(Action<IEnumerable<T>> collection)
    {
        _collectionChangedListeners.Remove(collection);
    }

    public void UnsubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved)
    {
        _listenersItemAdded.Remove(onItemAdded);
        _listenersItemRemoved.Remove(onItemRemoved);
    }

    public void UnsubscribeOnItemAdded(Action<T> onItemAdded)
    {
        _listenersItemAdded.Remove(onItemAdded);
    }

    public void UnsubscribeOnItemRemoved(Action<T> onItemRemoved)
    {
        _listenersItemRemoved.Remove(onItemRemoved);
    }

    public void Add(T item)
    {
        _list.Add(item);
        
        NotifyItemAdded(item);
        NotifyCollectionChanged();
    }

    public void Clear()
    {
        _list.Clear();
        
        NotifyCollectionChanged();
    }

    public bool Contains(T item)
    {
        return _list.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _list.CopyTo(array, arrayIndex);
        
        NotifyCollectionChanged();
    }

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

    public int IndexOf(T item)
    {
        return _list.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        _list.Insert(index, item);
        NotifyItemAddedAtIndex(item, index);
        NotifyCollectionChanged();
    }

    public void RemoveAt(int index)
    {
        var itemToRemove = _list[index];
        _list.RemoveAt(index);
        
        NotifyItemRemovedAtIndex(itemToRemove, index);
        NotifyCollectionChanged();
    }

    public T this[int index]
    {
        get => _list[index];
        set => _list[index] = value;
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
}
}
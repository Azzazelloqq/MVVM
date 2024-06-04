using System;
using System.Collections;
using System.Collections.Generic;

namespace MVVM.MVVM.ReactiveLibrary.Collections.Queue
{
public class ReactiveQueue<T> : IReactiveQueue<T>
{
    public int Count => _queue.Count;
    public bool IsReadOnly => ((ICollection<T>)_queue).IsReadOnly;
    public bool IsDisposed { get; private set; }

    private readonly Queue<T> _queue;
    private readonly List<Action<T>> _itemAddedActions;
    private readonly List<Action<T>> _itemRemovedActions;
    private readonly List<Action<IEnumerable<T>>> _collectionChangedListeners;

    public ReactiveQueue(int listenersCapacity = 30)
    {
        _queue = new Queue<T>();
        
        _itemAddedActions = new List<Action<T>>(listenersCapacity);
        _itemRemovedActions = new List<Action<T>>(listenersCapacity);
        _collectionChangedListeners = new List<Action<IEnumerable<T>>>();
    }

    public ReactiveQueue(IEnumerable<T> collection, int listenersCapacity = 30)
    {
        _queue = new Queue<T>(collection);
        
        _itemAddedActions = new List<Action<T>>(listenersCapacity);
        _itemRemovedActions = new List<Action<T>>(listenersCapacity);
        _collectionChangedListeners = new List<Action<IEnumerable<T>>>();
    }

    public ReactiveQueue(int capacity, int listenersCapacity = 30)
    {
        _queue = new Queue<T>(capacity);
        
        _itemAddedActions = new List<Action<T>>(listenersCapacity);
        _itemRemovedActions = new List<Action<T>>(listenersCapacity);
        _collectionChangedListeners = new List<Action<IEnumerable<T>>>();
    }
    public IEnumerator<T> GetEnumerator()
    {
        return _queue.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }
        
        _queue.Clear();
        IsDisposed = true;
    }

    public void SubscribeOnItemAdded(Action<T> onItemAdded)
    {
        _itemAddedActions.Add(onItemAdded);
    }

    public void SubscribeOnItemRemoved(Action<T> onItemRemoved)
    {
        _itemRemovedActions.Add(onItemRemoved);
    }

    public void SubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved)
    {
        _itemAddedActions.Add(onItemAdded);
        _itemRemovedActions.Add(onItemRemoved);
    }

    public void SubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged, bool notifyOnSubscribe = true)
    {
        if (notifyOnSubscribe)
        {
            collectionChanged?.Invoke(_queue);
        }
        
        _collectionChangedListeners.Add(collectionChanged);
    }

    public void UnsubscribeOnCollectionChanged(Action<IEnumerable<T>> collection)
    {
        _collectionChangedListeners.Remove(collection);
    }

    public void UnsubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved)
    {
        _itemAddedActions.Remove(onItemAdded);
        _itemRemovedActions.Remove(onItemRemoved);
    }

    public void UnsubscribeOnItemAdded(Action<T> onItemAdded)
    {
        _itemAddedActions.Remove(onItemAdded);
    }

    public void UnsubscribeOnItemRemoved(Action<T> onItemRemoved)
    {
        _itemRemovedActions.Remove(onItemRemoved);
    }

    [Obsolete("An queue does not support adding an element. Use" + nameof(Enqueue), true)]
    public void Add(T item)
    {
        throw new NotImplementedException("An queue does not support adding an element. Use" + nameof(Enqueue));
    }

    public void Clear()
    {
        _queue.Clear();
        
        NotifyCollectionChanged();
    }

    public bool Contains(T item)
    {
        return _queue.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _queue.CopyTo(array, arrayIndex);
        
        NotifyCollectionChanged();
    }

    [Obsolete("An queue does not support removing an element. Use" + nameof(Dequeue), true)]
    public bool Remove(T item)
    {
        throw new NotImplementedException("An queue does not support removing an element. Use" + nameof(Dequeue));
    }
    
    public object Clone()
    {
        var clone = new Queue<T>(_queue);
        
        return clone;
    }

    public T Dequeue()
    {
        var dequeue = _queue.Dequeue();

        NotifyItemRemoved(dequeue);
        NotifyCollectionChanged();

        return dequeue;
    }

    public void Enqueue(T item)
    {
        _queue.Enqueue(item);
        
        NotifyItemAdded(item);
        NotifyCollectionChanged();
    }

    public T Peek()
    {
        return _queue.Peek();
    }

    public T[] ToArray()
    {
        return _queue.ToArray();
    }

    public bool TryDequeue(out T result)
    {
        return _queue.TryDequeue(out result);
    }

    public bool TryPeek(out T result)
    {
        return _queue.TryPeek(out result);
    }
    
    private void NotifyItemAdded(T item)
    {
        foreach (var itemAddedAction in _itemAddedActions)
        {
            itemAddedAction.Invoke(item);
        }
    }

    private void NotifyItemRemoved(T item)
    {
        foreach (var itemRemovedAction in _itemRemovedActions)
        {
            itemRemovedAction.Invoke(item);
        }
    }

    private void NotifyCollectionChanged()
    {
        foreach (var collectionChangedListener in _collectionChangedListeners)
        {
            collectionChangedListener.Invoke(_queue);
        }
    }
}
}
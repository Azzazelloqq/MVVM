using System;
using System.Collections;
using System.Collections.Generic;
using Azzazelloqq.MVVM.ReactiveLibrary.Callbacks;

namespace Azzazelloqq.MVVM.ReactiveLibrary.Collections
{
/// <summary>
/// Represents a reactive queue that provides notifications for item enqueuing, dequeuing, 
/// and collection changes. Implements <see cref="IReactiveQueue{T}"/>.
/// </summary>
/// <typeparam name="T">The type of elements stored in the queue.</typeparam>
public class ReactiveQueue<T> : IReactiveQueue<T>
{
    /// <inheritdoc/>
    public int Count => _queue.Count;
   
    /// <inheritdoc/>
    public bool IsReadOnly => ((ICollection<T>)_queue).IsReadOnly;
   
    /// <inheritdoc/>
    public bool IsDisposed { get; private set; }

    
    private readonly ICallbacks<T> _itemAddedActions;
    private readonly ICallbacks<T> _itemRemovedActions;
    private readonly ICallbacks<IEnumerable<T>> _collectionChangedListeners;
    
    private readonly Queue<T> _queue;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveQueue{T}"/> class with the default capacity.
    /// </summary>
    /// <param name="listenersCapacity">The initial capacity for event listeners.</param>
    public ReactiveQueue(int listenersCapacity = 30)
    {
        _queue = new Queue<T>();
        
        _itemAddedActions = new CallbackBuffer<T>(listenersCapacity);
        _itemRemovedActions = new CallbackBuffer<T>(listenersCapacity);
        _collectionChangedListeners = new CallbackBuffer<IEnumerable<T>>(listenersCapacity);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveQueue{T}"/> class with a specified collection.
    /// </summary>
    /// <param name="collection">The collection of elements to initialize the queue with.</param>
    /// <param name="listenersCapacity">The initial capacity for event listeners.</param>
    public ReactiveQueue(IEnumerable<T> collection, int listenersCapacity = 30)
    {
        _queue = new Queue<T>(collection);
        
        _itemAddedActions = new CallbackBuffer<T>(listenersCapacity);
        _itemRemovedActions = new CallbackBuffer<T>(listenersCapacity);
        _collectionChangedListeners = new CallbackBuffer<IEnumerable<T>>(listenersCapacity);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveQueue{T}"/> class with a specified capacity.
    /// </summary>
    /// <param name="capacity">The initial capacity of the queue.</param>
    /// <param name="listenersCapacity">The initial capacity for event listeners.</param>
    public ReactiveQueue(int capacity, int listenersCapacity = 30)
    {
        _queue = new Queue<T>(capacity);
        _itemAddedActions = new CallbackBuffer<T>(listenersCapacity);
        _itemRemovedActions = new CallbackBuffer<T>(listenersCapacity);
        _collectionChangedListeners = new CallbackBuffer<IEnumerable<T>>(listenersCapacity);
    }
    
    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        return _queue.GetEnumerator();
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
        
        _queue.Clear();

        _collectionChangedListeners.Dispose();
        _itemAddedActions.Dispose();
        _itemRemovedActions.Dispose();
        
        IsDisposed = true;
    }

    /// <inheritdoc/>
    public void SubscribeOnItemAdded(Action<T> onItemAdded)
    {
        _itemAddedActions.Subscribe(onItemAdded);
    }

    /// <inheritdoc/>
    public void SubscribeOnItemRemoved(Action<T> onItemRemoved)
    {
        _itemRemovedActions.Subscribe(onItemRemoved);
    }

    /// <inheritdoc/>
    public void SubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved)
    {
        _itemAddedActions.Subscribe(onItemAdded);
        _itemRemovedActions.Subscribe(onItemRemoved);
    }

    /// <inheritdoc/>
    public void SubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged, bool notifyOnSubscribe = true)
    {
        if (notifyOnSubscribe)
        {
            collectionChanged?.Invoke(_queue);
        }
        
        _collectionChangedListeners.Subscribe(collectionChanged);
    }

    /// <inheritdoc/>
    public void UnsubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged)
    {
        _collectionChangedListeners.Unsubscribe(collectionChanged);
    }

    /// <inheritdoc/>
    public void UnsubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved)
    {
        _itemAddedActions.Unsubscribe(onItemAdded);
        _itemRemovedActions.Unsubscribe(onItemRemoved);
    }

    /// <inheritdoc/>
    public void UnsubscribeOnItemAdded(Action<T> onItemAdded)
    {
        _itemAddedActions.Unsubscribe(onItemAdded);
    }

    /// <inheritdoc/>
    public void UnsubscribeOnItemRemoved(Action<T> onItemRemoved)
    {
        _itemRemovedActions.Unsubscribe(onItemRemoved);
    }

    /// <summary>
    /// Obsolete method. A queue does not support adding elements directly.
    /// Use <see cref="Enqueue"/> instead.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <exception cref="NotImplementedException">Always throws an exception.</exception>
    [Obsolete("A queue does not support adding an element. Use " + nameof(Enqueue), true)]
    public void Add(T item)
    {
        throw new NotImplementedException("An queue does not support adding an element. Use" + nameof(Enqueue));
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _queue.Clear();
        
        NotifyCollectionChanged();
    }

    /// <inheritdoc/>
    public bool Contains(T item)
    {
        return _queue.Contains(item);
    }

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
    {
        _queue.CopyTo(array, arrayIndex);
        
        NotifyCollectionChanged();
    }

    /// <summary>
    /// Obsolete method. A queue does not support removing elements directly.
    /// Use <see cref="Dequeue"/> instead.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <returns><c>false</c> always.</returns>
    /// <exception cref="NotImplementedException">Always throws an exception.</exception>
    [Obsolete("A queue does not support removing an element. Use " + nameof(Dequeue), true)]
    public bool Remove(T item)
    {
        throw new NotImplementedException("An queue does not support removing an element. Use" + nameof(Dequeue));
    }
    
    /// <inheritdoc/>
    public object Clone()
    {
        var clone = new ReactiveQueue<T>(_queue);
        
        return clone;
    }

    /// <inheritdoc/>
    public T Dequeue()
    {
        var dequeue = _queue.Dequeue();

        NotifyItemRemoved(dequeue);
        NotifyCollectionChanged();

        return dequeue;
    }

    /// <inheritdoc/>
    public void Enqueue(T item)
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(ReactiveQueue<T>));
        }
        
        _queue.Enqueue(item);
        
        NotifyItemAdded(item);
        NotifyCollectionChanged();
    }

    /// <inheritdoc/>
    public T Peek()
    {
        return _queue.Peek();
    }

    /// <inheritdoc/>
    public T[] ToArray()
    {
        return _queue.ToArray();
    }

    /// <inheritdoc/>
    public bool TryDequeue(out T result)
    {
        return _queue.TryDequeue(out result);
    }

    /// <inheritdoc/>
    public bool TryPeek(out T result)
    {
        return _queue.TryPeek(out result);
    }
    
    private void NotifyItemAdded(T item)
    {
        _itemAddedActions.Notify(item);
    }

    private void NotifyItemRemoved(T item)
    {
        _itemRemovedActions.Notify(item);
    }

    private void NotifyCollectionChanged()
    {
        _collectionChangedListeners.Notify(_queue);
    }
}
}
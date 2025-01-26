using System;
using System.Collections;
using System.Collections.Generic;

namespace MVVM.MVVM.ReactiveLibrary.Collections.Stack
{
/// <summary>
/// Represents a reactive stack that provides notifications for operations such as pushing, popping, 
/// and peeking at elements in the stack. Implements <see cref="IReactiveStack{T}"/>.
/// </summary>
/// <typeparam name="T">The type of elements stored in the stack.</typeparam>
public class ReactiveStack<T> : IReactiveStack<T>
{
    /// <inheritdoc/>
    public int Count => _stack.Count;
    
    /// <inheritdoc/>
    public bool IsReadOnly => ((ICollection<T>)_stack).IsReadOnly;
    
    /// <inheritdoc/>
    public bool IsDisposed { get; private set; }

    private List<Action<T>> ItemAddedActions => _itemAddedActions ??= new List<Action<T>>(_listenersCapacity);
    private List<Action<T>> ItemRemovedActions => _itemRemovedActions ??= new List<Action<T>>(_listenersCapacity);
    private List<Action<IEnumerable<T>>> CollectionChangedListeners => _collectionChangedListeners ??= new List<Action<IEnumerable<T>>>(_listenersCapacity);

    private List<Action<T>> _itemAddedActions;
    private List<Action<T>> _itemRemovedActions;
    private List<Action<IEnumerable<T>>> _collectionChangedListeners;

    private readonly Stack<T> _stack;
    private readonly int _listenersCapacity;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveStack{T}"/> class with the default capacity.
    /// </summary>
    /// <param name="listenersCapacity">The initial capacity for event listeners.</param>
    public ReactiveStack(int listenersCapacity = 30)
    {
        _stack = new Stack<T>();
     
        _listenersCapacity = listenersCapacity;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveStack{T}"/> class with a specified collection.
    /// </summary>
    /// <param name="collection">The collection of elements to initialize the stack with.</param>
    /// <param name="listenersCapacity">The initial capacity for event listeners.</param>
    public ReactiveStack(IEnumerable<T> collection, int listenersCapacity = 30)
    {
        _stack = new Stack<T>(collection);
        
        _listenersCapacity = listenersCapacity;
    }

    
    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveStack{T}"/> class with a specified capacity.
    /// </summary>
    /// <param name="capacity">The initial capacity of the stack.</param>
    /// <param name="listenersCapacity">The initial capacity for event listeners.</param>
    public ReactiveStack(int capacity, int listenersCapacity = 30)
    {
        _stack = new Stack<T>(capacity);
        
        _listenersCapacity = listenersCapacity;
    }
    
    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        return _stack.GetEnumerator();
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
        
        _stack.Clear();
        
        CollectionChangedListeners.Clear();
        ItemAddedActions.Clear();
        ItemRemovedActions.Clear();
        
        IsDisposed = true;
    }

    /// <inheritdoc/>
    public void SubscribeOnItemAdded(Action<T> onItemAdded)
    {
        ItemAddedActions.Add(onItemAdded);
    }

    /// <inheritdoc/>
    public void SubscribeOnItemRemoved(Action<T> onItemRemoved)
    {
        ItemRemovedActions.Add(onItemRemoved);
    }

    /// <inheritdoc/>
    public void SubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved)
    {
        ItemAddedActions.Add(onItemAdded);
        ItemRemovedActions.Add(onItemRemoved);
    }

    /// <inheritdoc/>
    public void SubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged, bool notifyOnSubscribe = true)
    {
        if (notifyOnSubscribe)
        {
            collectionChanged.Invoke(_stack);
        }
        
        CollectionChangedListeners.Add(collectionChanged);
    }

    /// <inheritdoc/>
    public void UnsubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged)
    {
        CollectionChangedListeners.Remove(collectionChanged);
    }

    /// <inheritdoc/>
    public void UnsubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved)
    {
        ItemAddedActions.Remove(onItemAdded);
        ItemRemovedActions.Remove(onItemRemoved);
    }

    /// <inheritdoc/>
    public void UnsubscribeOnItemAdded(Action<T> onItemAdded)
    {
        ItemAddedActions.Remove(onItemAdded);
    }

    /// <inheritdoc/>
    public void UnsubscribeOnItemRemoved(Action<T> onItemRemoved)
    {
        ItemRemovedActions.Remove(onItemRemoved);
    }

    /// <summary>
    /// Obsolete method. A stack does not support adding elements directly.
    /// Its invoke <see cref="Push"/> as default 
    /// Use <see cref="Push"/> instead.
    /// </summary>
    /// <param name="item">The item to add.</param>
    [Obsolete("A stack does not support adding an element. Use " + nameof(Push), true)]
    public void Add(T item)
    {
        Push(item);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _stack.Clear();
        NotifyCollectionChanged();
    }

    /// <inheritdoc/>
    public bool Contains(T item)
    {
        return _stack.Contains(item);
    }

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
    {
        _stack.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Obsolete method. A stack does not support removing elements directly.
    /// Use <see cref="Pop"/> instead.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <returns><c>false</c> always.</returns>
    /// <exception cref="NotImplementedException">Always throws an exception.</exception>
    [Obsolete("A stack does not support removing an element. Use " + nameof(Pop), true)]
    public bool Remove(T item)
    {
        throw new NotImplementedException("A stack does not support removing an element. Use" + nameof(Pop));
    }
    
    /// <inheritdoc/>
    public T Peek()
    {
        return _stack.Peek();
    }

    /// <inheritdoc/>
    public T Pop()
    {
        var pop = _stack.Pop();
        NotifyItemRemoved(pop);
        NotifyCollectionChanged();

        return pop;
    }

    /// <inheritdoc/>
    public void Push(T item)
    {
        _stack.Push(item);
        
        NotifyItemAdded(item);
        NotifyCollectionChanged();
    }

    /// <inheritdoc/>
    public T[] ToArray()
    {
        return _stack.ToArray();
    }

    /// <inheritdoc/>
    public bool TryPeek(out T result)
    {
        return _stack.TryPeek(out result);
    }

    /// <inheritdoc/>
    public bool TryPop(out T result)
    {
        var isSuccess = _stack.TryPop(out result);

        if (!isSuccess)
        {
            return false;
        }

        NotifyItemRemoved(result);
        NotifyCollectionChanged();

        return true;
    }
    
    private void NotifyItemAdded(T item)
    {
        foreach (var itemAddedAction in ItemAddedActions)
        {
            itemAddedAction.Invoke(item);
        }
    }

    private void NotifyItemRemoved(T item)
    {
        foreach (var itemRemovedAction in ItemRemovedActions)
        {
            itemRemovedAction.Invoke(item);
        }
    }

    private void NotifyCollectionChanged()
    {
        foreach (var collectionChangedListener in CollectionChangedListeners)
        {
            collectionChangedListener.Invoke(_stack);
        }
    }
}
}
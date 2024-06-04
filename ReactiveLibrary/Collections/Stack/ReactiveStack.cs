using System;
using System.Collections;
using System.Collections.Generic;

namespace MVVM.MVVM.ReactiveLibrary.Collections.Stack
{
public class ReactiveStack<T> : IReactiveStack<T>
{
    public int Count => _stack.Count;
    public bool IsReadOnly => ((ICollection<T>)_stack).IsReadOnly;
    public bool IsDisposed { get; private set; }

    private readonly Stack<T> _stack;
    private readonly List<Action<T>> _itemAddedActions;
    private readonly List<Action<T>> _itemRemovedActions;
    private readonly List<Action<IEnumerable<T>>> _collectionChangedListeners;

    public ReactiveStack(int listenersCapacity = 30)
    {
        _stack = new Stack<T>();
        
        _itemAddedActions = new List<Action<T>>(listenersCapacity);
        _itemRemovedActions = new List<Action<T>>(listenersCapacity);
        _collectionChangedListeners = new List<Action<IEnumerable<T>>>();
    }

    public ReactiveStack(IEnumerable<T> collection, int listenersCapacity = 30)
    {
        _stack = new Stack<T>(collection);
        
        _itemAddedActions = new List<Action<T>>(listenersCapacity);
        _itemRemovedActions = new List<Action<T>>(listenersCapacity);
        _collectionChangedListeners = new List<Action<IEnumerable<T>>>();
    }

    public ReactiveStack(int capacity, int listenersCapacity = 30)
    {
        _stack = new Stack<T>(capacity);
        
        _itemAddedActions = new List<Action<T>>(listenersCapacity);
        _itemRemovedActions = new List<Action<T>>(listenersCapacity);
        _collectionChangedListeners = new List<Action<IEnumerable<T>>>();
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        return _stack.GetEnumerator();
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
        
        _stack.Clear();
        
        _collectionChangedListeners.Clear();
        _itemAddedActions.Clear();
        _itemRemovedActions.Clear();
        
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
            collectionChanged.Invoke(_stack);
        }
        
        _collectionChangedListeners.Add(collectionChanged);
    }

    public void UnsubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged)
    {
        _collectionChangedListeners.Remove(collectionChanged);
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

    [Obsolete("A stack does not support adding an element. Use" + nameof(Push), true)]
    public void Add(T item)
    {
        throw new NotImplementedException("A stack does not support adding an element. Use" + nameof(Push));
    }

    public void Clear()
    {
        _stack.Clear();
        NotifyCollectionChanged();
    }

    public bool Contains(T item)
    {
        return _stack.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _stack.CopyTo(array, arrayIndex);
    }

    [Obsolete("A stack does not support removing an element. Use" + nameof(Pop), true)]
    public bool Remove(T item)
    {
        throw new NotImplementedException("A stack does not support removing an element. Use" + nameof(Pop));
    }
    
    public T Peek()
    {
        return _stack.Peek();
    }

    public T Pop()
    {
        var pop = _stack.Pop();
        NotifyItemRemoved(pop);
        NotifyCollectionChanged();

        return pop;
    }

    public void Push(T item)
    {
        _stack.Push(item);
        
        NotifyItemAdded(item);
        NotifyCollectionChanged();
    }

    public T[] ToArray()
    {
        return _stack.ToArray();
    }

    public bool TryPeek(out T result)
    {
        return _stack.TryPeek(out result);
    }

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
            collectionChangedListener.Invoke(_stack);
        }
    }
}
}
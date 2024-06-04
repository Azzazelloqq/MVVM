using System;
using System.Collections;
using System.Collections.Generic;
using MVVM.MVVM.ReactiveLibrary.Collections.Base;
using UnityEngine.PlayerLoop;

namespace MVVM.MVVM.ReactiveLibrary.Collections.Array
{
public class ReactiveArray<T> : IReactiveArray<T>
{
    public int Count => Length;
    public int Length => _array.Length;
    public bool IsReadOnly => _array.IsReadOnly;
    public bool IsDisposed { get; private set; }

    private T[] _array;
    private List<Action<T, int>> _itemChangedByIndexListeners;
    private List<Action<IEnumerable<T>>> _collectionChangedListeners;

    public ReactiveArray(int length, int listenersCapacity = 30)
    {
        _array = new T[length];
        _itemChangedByIndexListeners = new List<Action<T, int>>(listenersCapacity);
        _collectionChangedListeners = new List<Action<IEnumerable<T>>>(listenersCapacity);
    }

    public ReactiveArray(IReadOnlyList<T> array, int listenersCapacity = 30)
    {
        _array = new T[array.Count];
        for (var i = 0; i < array.Count; i++)
        {
            _array[i] = array[i];
        }
        
        _itemChangedByIndexListeners = new List<Action<T, int>>(listenersCapacity);
        _collectionChangedListeners = new List<Action<IEnumerable<T>>>(listenersCapacity);
    }

    public void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }
        
        _array = System.Array.Empty<T>();
        IsDisposed = true;
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var item in _array)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _array.GetEnumerator();
    }

    [Obsolete("An array does not support adding an element", true)]
    public void SubscribeOnItemAdded(Action<T> onItemAdded)
    {
        throw new Exception("An array does not support adding an element");
    }
    
    [Obsolete("An array does not support removing an element", true)]

    public void SubscribeOnItemRemoved(Action<T> onItemRemoved)
    {
        throw new Exception("An array does not support removing an element");
    }

    [Obsolete("An array does not support adding and removing an element", true)]
    public void SubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved)
    {
        throw new Exception("An array does not support adding and removing an element");
    }

    public void SubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged, bool notifyOnSubscribe = true)
    {
        _collectionChangedListeners.Add(collectionChanged);
    }

    public void SubscribeOnItemChangedByIndex(Action<T, int> onItemChangedByIndex)
    {
        _itemChangedByIndexListeners.Add(onItemChangedByIndex);
    }

    public void UnsubscribeOnItemChangedByIndex(Action<T, int> onItemChangedByIndex)
    {
        _itemChangedByIndexListeners.Remove(onItemChangedByIndex);
    }

    public void UnsubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged)
    {
        _collectionChangedListeners.Remove(collectionChanged);
    }

    [Obsolete("An array does not support adding and removing an element", true)]
    public void UnsubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved)
    {
        throw new Exception("An array does not support adding and removing an element");
    }

    [Obsolete("An array does not support adding an element", true)]
    public void UnsubscribeOnItemAdded(Action<T> onItemAdded)
    {
        throw new Exception("An array does not support adding an element");
    }

    [Obsolete("An array does not support removing an element", true)]
    public void UnsubscribeOnItemRemoved(Action<T> onItemRemoved)
    {
        throw new Exception("An array does not support removing an element");
    }
    
    public T[] Clone()
    {
        return (T[]) _array.Clone();
    }

    [Obsolete("An array does not support adding an element", true)]
    public void Add(T item)
    {
        throw new Exception("An array does not support adding an element");
    }

    public void Clear()
    {
        _array = System.Array.Empty<T>();
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

    [Obsolete("An array does not support removing an element", true)]
    public bool Remove(T item)
    {
        throw new Exception("An array does not support removing an element");
    }
    
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

    [Obsolete("An array does not support adding an element", true)]
    public void Insert(int index, T item)
    {
        throw new Exception("An array does not support adding an element");
    }

    [Obsolete("An array does not support removing an element", true)]
    public void RemoveAt(int index)
    {
        throw new Exception("An array does not support removing an element");
    }

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

    public int BinarySearch(int index, int length, T value)
    {
        return System.Array.BinarySearch(_array, index, length, value);
    }

    public int BinarySearch(int index, int length, T value, IComparer<T> comparer)
    {
        return System.Array.BinarySearch(_array, index, length, value, comparer);
    }

    public int BinarySearch(T value)
    {
        return System.Array.BinarySearch(_array, value);
    }

    public int BinarySearch(T value, IComparer<T> comparer)
    {
        return System.Array.BinarySearch(_array, value, comparer);
    }

    public void ForEach(Action<T> action)
    {
        foreach (var element in _array)
        {
            action(element);
        }
    }

    public void Sort()
    {
        System.Array.Sort(_array);
        NotifyCollectionChanged();
    }

    public void Sort(IComparer<T> comparer)
    {
        System.Array.Sort(_array, comparer);
        NotifyCollectionChanged();
    }

    public void Sort(Comparison<T> comparison)
    {
        System.Array.Sort(_array, comparison);
        NotifyCollectionChanged();
    }

    public void Sort(int index, int length)
    {
        System.Array.Sort(_array, index, length);
        NotifyCollectionChanged();
    }

    public void Sort(int index, int length, IComparer<T> comparer)
    {
        System.Array.Sort(_array, index, length, comparer);
        NotifyCollectionChanged();
    }

    public void Reverse()
    {
        System.Array.Reverse(_array);
        NotifyCollectionChanged();
    }

    public void Reverse(int index, int length)
    {
        System.Array.Reverse(_array, index, length);
        NotifyCollectionChanged();
    }

    public T FindLast(Predicate<T> match)
    {
        return System.Array.FindLast(_array, match);
    }

    public T Find(Predicate<T> match)
    {
        return System.Array.Find(_array, match);
    }

    public IReadOnlyReactiveCollection<T> AsReadOnly()
    {
        return this;
    }

    public IReactiveArray<T> CloneAsReactive()
    {
        return new ReactiveArray<T>(_array);
    }

    private void NotifyItemChangedByIndex(T item, int index)
    {
        foreach (var itemChangedByIndexListener in _itemChangedByIndexListeners)
        {
            itemChangedByIndexListener.Invoke(item, index);
        }
    }

    private void NotifyCollectionChanged()
    {
        foreach (var collectionChangedListener in _collectionChangedListeners)
        {
            collectionChangedListener.Invoke(_array);
        }
    }
}
}
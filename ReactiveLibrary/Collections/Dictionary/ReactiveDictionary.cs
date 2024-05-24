using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVM.MVVM.ReactiveLibrary.Collections.Dictionary
{
public class ReactiveDictionary<TKey, TValue> : IReactiveDictionary<TKey, TValue>
{
    public ICollection<TKey> Keys => _dictionary.Keys;
    public ICollection<TValue> Values => _dictionary.Values;
    public bool IsDisposed { get; private set; }
    public int Count => _dictionary.Count;
    public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).IsReadOnly;

    private readonly List<Action<KeyValuePair<TKey, TValue>>> _itemAddedListeners;
    private readonly List<Action<KeyValuePair<TKey, TValue>>> _itemRemovedListeners;
    private readonly List<Action<IEnumerable<KeyValuePair<TKey, TValue>>>> _collectionChangedListeners;
    private readonly Dictionary<TKey, TValue> _dictionary;

    public ReactiveDictionary(int capacity, int listenersCapacity = 30)
    {
        _dictionary = new Dictionary<TKey, TValue>(capacity);
        _itemAddedListeners = new List<Action<KeyValuePair<TKey, TValue>>>(listenersCapacity);
        _itemRemovedListeners = new List<Action<KeyValuePair<TKey, TValue>>>(listenersCapacity);
        _collectionChangedListeners = new List<Action<IEnumerable<KeyValuePair<TKey, TValue>>>>(listenersCapacity);
    }

    public ReactiveDictionary(int capacity, IEqualityComparer<TKey> comparer, int listenersCapacity = 30)
    {
        _dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        _itemAddedListeners = new List<Action<KeyValuePair<TKey, TValue>>>(listenersCapacity);
        _itemRemovedListeners = new List<Action<KeyValuePair<TKey, TValue>>>(listenersCapacity);
        _collectionChangedListeners = new List<Action<IEnumerable<KeyValuePair<TKey, TValue>>>>(listenersCapacity);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
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

        IsDisposed = true;
        _dictionary.Clear();
        _itemAddedListeners.Clear();
        _itemRemovedListeners.Clear();
        _collectionChangedListeners.Clear();
    }

    public void SubscribeOnItemAdded(Action<KeyValuePair<TKey, TValue>> onItemAdded)
    {
        _itemAddedListeners.Add(onItemAdded);
    }

    public void SubscribeOnItemRemoved(Action<KeyValuePair<TKey, TValue>> onItemRemoved)
    {
        _itemRemovedListeners.Add(onItemRemoved);
    }

    public void SubscribeOnCollectionChanged(Action<KeyValuePair<TKey, TValue>> onItemAdded,
        Action<KeyValuePair<TKey, TValue>> onItemRemoved)
    {
        _itemAddedListeners.Add(onItemAdded);
        _itemRemovedListeners.Add(onItemRemoved);
    }

    public void SubscribeOnCollectionChanged(Action<IEnumerable<KeyValuePair<TKey, TValue>>> collectionChanged,
        bool notifyOnSubscribe = true)
    {
        _collectionChangedListeners.Add(collectionChanged);

        if (notifyOnSubscribe)
        {
            collectionChanged.Invoke(_dictionary);
        }
    }

    public void UnsubscribeOnCollectionChanged(Action<IEnumerable<KeyValuePair<TKey, TValue>>> collection)
    {
        _collectionChangedListeners.Remove(collection);
    }

    public void UnsubscribeOnCollectionChanged(Action<KeyValuePair<TKey, TValue>> onItemAdded,
        Action<KeyValuePair<TKey, TValue>> onItemRemoved)
    {
        _itemAddedListeners.Remove(onItemAdded);
        _itemRemovedListeners.Remove(onItemRemoved);
    }

    public void UnsubscribeOnItemAdded(Action<KeyValuePair<TKey, TValue>> onItemAdded)
    {
        _itemAddedListeners.Remove(onItemAdded);
    }

    public void UnsubscribeOnItemRemoved(Action<KeyValuePair<TKey, TValue>> onItemRemoved)
    {
        _itemRemovedListeners.Remove(onItemRemoved);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        _dictionary.Add(item.Key, item.Value);

        NotifyItemAdded(item);
        NotifyCollectionChanged();
    }

    public void Clear()
    {
        _dictionary.Clear();

        NotifyCollectionChanged();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return _dictionary.Contains(item);
    }

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

    public void Add(TKey key, TValue value)
    {
        _dictionary.Add(key, value);

        NotifyItemAdded(new KeyValuePair<TKey, TValue>(key, value));
        NotifyCollectionChanged();
    }

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

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

    public bool TryGetValue(TKey key, out TValue value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    public TValue this[TKey key]
    {
        get => _dictionary[key];
        set
        {
            var containsKey = _dictionary.ContainsKey(key);
            _dictionary[key] = value;
            if (containsKey)
            {
                return;
            }

            NotifyItemAdded(new KeyValuePair<TKey, TValue>(key, value));
            NotifyCollectionChanged();
        }
    }

    private void NotifyItemAdded(KeyValuePair<TKey, TValue> item)
    {
        foreach (var itemAddedListener in _itemAddedListeners)
        {
            itemAddedListener.Invoke(item);
        }
    }

    private void NotifyItemRemoved(KeyValuePair<TKey, TValue> item)
    {
        foreach (var itemRemovedListener in _itemRemovedListeners)
        {
            itemRemovedListener.Invoke(item);
        }
    }

    private void NotifyCollectionChanged()
    {
        foreach (var collectionChangedListener in _collectionChangedListeners)
        {
            collectionChangedListener.Invoke(_dictionary);
        }
    }
}
}
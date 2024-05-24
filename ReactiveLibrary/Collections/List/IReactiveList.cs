using System;
using System.Collections.Generic;
using MVVM.MVVM.ReactiveLibrary.Collections.Base;

namespace MVVM.MVVM.ReactiveLibrary.Collections.List
{
public interface IReactiveList<T> : IReactiveCollection<T>, IList<T>
{
    public void Sort();
    public void Sort(IComparer<T> comparer);
    public void Sort(Comparison<T> comparison);
    public void Sort(int index, int count, IComparer<T> comparer);
    public int IndexOf(T item, int index);
    public int IndexOf(T item, int index, int count);
    public int LastIndexOf(T item);
    public int LastIndexOf(T item, int index);
    public int LastIndexOf(T item, int index, int count);
    public void RemoveRange(int index, int count);
    public void Reverse();
    public void Reverse(int index, int count);
    public T[] ToArray();
    public void ForEach(Action<T> action);
    public bool Exists(Predicate<T> match);
    public int BinarySearch(int index, int count, T item, IComparer<T> comparer);
    public int BinarySearch(T item);
    public int BinarySearch(T item, IComparer<T> comparer);
    public IReadOnlyReactiveCollection<T> AsReadOnly();
    public void AddRange(IEnumerable<T> collection);

    public void SubscribeOnItemAddedByIndex(Action<T, int> onItemAdded);
    public void SubscribeOnItemRemovedByIndex(Action<T, int> onItemRemoved);
    public void SubscribeOnItemChangedByIndex(Action<T, int> onItemChanged);
    
    public void UnsubscribeOnItemChangedByIndex(Action<T, int> onItemChanged);
    public void UnsubscribeOnItemAddedByIndex(Action<T, int> onItemAdded);
    public void UnsubscribeOnItemRemovedByIndex(Action<T, int> onItemRemoved);
}
}
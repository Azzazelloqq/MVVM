using System;
using System.Collections.Generic;
using MVVM.MVVM.ReactiveLibrary.Collections.Base;

namespace MVVM.MVVM.ReactiveLibrary.Collections.Array
{
public interface IReactiveArray<T> : IReactiveCollection<T>, IList<T>
{
    public int Length { get;}
    public int BinarySearch(int index, int length, T value);
    public int BinarySearch(int index, int length, T value,
        IComparer<T> comparer);
    public int BinarySearch(T value);
    public int BinarySearch(T value, IComparer<T> comparer);
    public void ForEach(Action<T> action);
    public void Sort();
    public void Sort(IComparer<T> comparer);
    public void Sort(Comparison<T> comparison);
    public void Sort(int index, int length);
    public void Sort(int index, int length, IComparer<T> comparer);
    public void Reverse();
    public void Reverse(int index, int length);
    public T FindLast(Predicate<T> match);
    public T Find(Predicate<T> match);
    public IReadOnlyReactiveCollection<T> AsReadOnly();
    public IReactiveArray<T> CloneAsReactive();
    public T[] Clone();

    public void SubscribeOnItemChangedByIndex(Action<T, int> onItemChangedByIndex);
    public void UnsubscribeOnItemChangedByIndex(Action<T, int> onItemChangedByIndex);
}
}
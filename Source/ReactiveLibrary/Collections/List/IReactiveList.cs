using System;
using System.Collections.Generic;
using Azzazelloqq.MVVM.Source.ReactiveLibrary.Collections.Base;

namespace Azzazelloqq.MVVM.Source.ReactiveLibrary.Collections.List
{
/// <summary>
/// Represents a reactive list that provides event notifications when items are added, removed, or changed.
/// Inherits from <see cref="IReactiveCollection{T}"/> and <see cref="IList{T}"/>.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public interface IReactiveList<T> : IReactiveCollection<T>, IList<T>
{
	/// <summary>
	/// Returns the first element of the list that satisfies the provided predicate.
	/// If no element satisfies the predicate, returns the default value of the type.
	/// </summary>
	/// <param name="predicate">The condition to evaluate for each element.</param>
	/// <returns>The first element that matches the condition or the default value if none found.</returns>
	public T FirstOrDefault(Func<T, bool> predicate);

	/// <summary>
	/// Removes all elements from the list that satisfy the specified predicate.
	/// </summary>
	/// <param name="predicate">The condition to evaluate for each element.</param>
	/// <returns>The number of elements removed.</returns>
	public int RemoveAll(Predicate<T> predicate);

	/// <summary>
	/// Removes element from the list that satisfy the specified predicate.
	/// </summary>
	/// <param name="predicate">The condition to evaluate for each element.</param>
	public void Remove(Predicate<T> predicate);
	
	/// <summary>
	/// Sorts the elements in the entire list.
	/// </summary>
	public void Sort();

	/// <summary>
	/// Sorts the elements in the entire list using the specified comparer.
	/// </summary>
	/// <param name="comparer">The comparer to use for comparing elements.</param>
	public void Sort(IComparer<T> comparer);

	/// <summary>
	/// Sorts the elements in the entire list using the specified comparison.
	/// </summary>
	/// <param name="comparison">The comparison to use for sorting elements.</param>
	public void Sort(Comparison<T> comparison);

	/// <summary>
	/// Sorts the elements in the specified range of the list using the specified comparer.
	/// </summary>
	/// <param name="index">The starting index of the range.</param>
	/// <param name="count">The number of elements to sort.</param>
	/// <param name="comparer">The comparer to use for comparing elements.</param>
	public void Sort(int index, int count, IComparer<T> comparer);

	/// <summary>
	/// Finds the index of the first occurrence of the specified item starting at the specified index.
	/// </summary>
	/// <param name="item">The item to locate.</param>
	/// <param name="index">The starting index for the search.</param>
	/// <returns>The index of the item if found; otherwise, -1.</returns>
	public int IndexOf(T item, int index);

	/// <summary>
	/// Finds the index of the first occurrence of the specified item in the specified range.
	/// </summary>
	/// <param name="item">The item to locate.</param>
	/// <param name="index">The starting index for the search.</param>
	/// <param name="count">The number of elements to include in the search.</param>
	/// <returns>The index of the item if found; otherwise, -1.</returns>
	public int IndexOf(T item, int index, int count);

	/// <summary>
	/// Finds the index of the last occurrence of the specified item.
	/// </summary>
	/// <param name="item">The item to locate.</param>
	/// <returns>The index of the item if found; otherwise, -1.</returns>
	public int LastIndexOf(T item);

	/// <summary>
	/// Finds the index of the last occurrence of the specified item starting at the specified index.
	/// </summary>
	/// <param name="item">The item to locate.</param>
	/// <param name="index">The starting index for the search.</param>
	/// <returns>The index of the item if found; otherwise, -1.</returns>
	public int LastIndexOf(T item, int index);

	/// <summary>
	/// Finds the index of the last occurrence of the specified item in the specified range.
	/// </summary>
	/// <param name="item">The item to locate.</param>
	/// <param name="index">The starting index for the search.</param>
	/// <param name="count">The number of elements to include in the search.</param>
	/// <returns>The index of the item if found; otherwise, -1.</returns>
	public int LastIndexOf(T item, int index, int count);

	/// <summary>
	/// Removes a range of elements from the list.
	/// </summary>
	/// <param name="index">The starting index of the range to remove.</param>
	/// <param name="count">The number of elements to remove.</param>
	public void RemoveRange(int index, int count);

	/// <summary>
	/// Reverses the order of the elements in the entire list.
	/// </summary>
	public void Reverse();

	/// <summary>
	/// Reverses the order of the elements in the specified range of the list.
	/// </summary>
	/// <param name="index">The starting index of the range.</param>
	/// <param name="count">The number of elements to reverse.</param>
	public void Reverse(int index, int count);

	/// <summary>
	/// Copies the elements of the list to a new array.
	/// </summary>
	/// <returns>An array containing the elements of the list.</returns>
	public T[] ToArray();

	/// <summary>
	/// Performs the specified action on each element of the list.
	/// </summary>
	/// <param name="action">The action to perform on each element.</param>
	public void ForEach(Action<T> action);

	/// <summary>
	/// Determines whether an element exists in the list that matches the specified predicate.
	/// </summary>
	/// <param name="match">The predicate used to match elements.</param>
	/// <returns><c>true</c> if an element is found; otherwise, <c>false</c>.</returns>
	public bool Exists(Predicate<T> match);

	/// <summary>
	/// Searches the list using a binary search algorithm, within the specified range, using the specified comparer.
	/// </summary>
	/// <param name="index">The starting index of the range.</param>
	/// <param name="count">The number of elements to include in the search.</param>
	/// <param name="item">The item to search for.</param>
	/// <param name="comparer">The comparer to use for comparing elements.</param>
	/// <returns>The index of the item if found; otherwise, -1.</returns>
	public int BinarySearch(int index, int count, T item, IComparer<T> comparer);

	/// <summary>
	/// Searches the entire list using a binary search algorithm.
	/// </summary>
	/// <param name="item">The item to search for.</param>
	/// <returns>The index of the item if found; otherwise, -1.</returns>
	public int BinarySearch(T item);

	/// <summary>
	/// Searches the entire list using a binary search algorithm and the specified comparer.
	/// </summary>
	/// <param name="item">The item to search for.</param>
	/// <param name="comparer">The comparer to use for comparing elements.</param>
	/// <returns>The index of the item if found; otherwise, -1.</returns>
	public int BinarySearch(T item, IComparer<T> comparer);

	/// <summary>
	/// Returns a read-only view of the list.
	/// </summary>
	/// <returns>A read-only <see cref="IReadOnlyReactiveCollection{T}"/>.</returns>
	public IReadOnlyReactiveCollection<T> AsReadOnly();

	/// <summary>
	/// Adds a range of elements to the list.
	/// </summary>
	/// <param name="collection">The collection of elements to add.</param>
	public void AddRange(IEnumerable<T> collection);

	/// <summary>
	/// Subscribes to notifications when an item is added to the list at a specific index.
	/// </summary>
	/// <param name="onItemAdded">The action to invoke when an item is added at the specified index.</param>
	public void SubscribeOnItemAddedByIndex(Action<(T, int)> onItemAdded);

	/// <summary>
	/// Subscribes to notifications when an item is removed from the list at a specific index.
	/// </summary>
	/// <param name="onItemRemoved">The action to invoke when an item is removed at the specified index.</param>
	public void SubscribeOnItemRemovedByIndex(Action<(T, int)> onItemRemoved);

	/// <summary>
	/// Subscribes to notifications when an item is changed in the list at a specific index.
	/// </summary>
	/// <param name="onItemChanged">The action to invoke when an item is changed at the specified index.</param>
	public void SubscribeOnItemChangedByIndex(Action<(T, int)> onItemChanged);

	/// <summary>
	/// Unsubscribes from notifications when an item is changed in the list at a specific index.
	/// </summary>
	/// <param name="onItemChanged">The action to unsubscribe from.</param>
	public void UnsubscribeOnItemChangedByIndex(Action<(T, int)> onItemChanged);

	/// <summary>
	/// Unsubscribes from notifications when an item is added to the list at a specific index.
	/// </summary>
	/// <param name="onItemAdded">The action to unsubscribe from.</param>
	public void UnsubscribeOnItemAddedByIndex(Action<(T, int)> onItemAdded);

	/// <summary>
	/// Unsubscribes from notifications when an item is removed from the list at a specific index.
	/// </summary>
	/// <param name="onItemRemoved">The action to unsubscribe from.</param>
	public void UnsubscribeOnItemRemovedByIndex(Action<(T, int)> onItemRemoved);
}
}
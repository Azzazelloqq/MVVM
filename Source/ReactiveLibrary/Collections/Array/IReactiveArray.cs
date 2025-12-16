#if !PROJECT_SUPPORT_R3
using System;
using System.Collections.Generic;
using Azzazelloqq.MVVM.ReactiveLibrary.Callbacks;

namespace Azzazelloqq.MVVM.ReactiveLibrary.Collections
{
/// <summary>
/// Represents a reactive array that notifies observers of changes.
/// </summary>
/// <typeparam name="T">The type of elements stored.</typeparam>
public interface IReactiveArray<T> : IReactiveCollection<T>, IReadOnlyList<T>
{
	/// <summary>
	/// Gets the total number of elements in the array.
	/// </summary>
	public int Length { get; }

	/// <summary>
	/// Searches a range of elements in the array for an element
	/// using the default comparer and returns the index.
	/// </summary>
	/// <param name="index">The starting index of the range.</param>
	/// <param name="length">The number of elements to search.</param>
	/// <param name="value">The object to locate.</param>
	/// <returns>
	/// The zero-based index of the element, if found; otherwise, a negative number.
	/// </returns>
	public int BinarySearch(int index, int length, T value);

	/// <summary>
	/// Searches a range of elements using a specified comparer and returns the index.
	/// </summary>
	/// <param name="index">The starting index of the range.</param>
	/// <param name="length">The number of elements to search.</param>
	/// <param name="value">The object to locate.</param>
	/// <param name="comparer">
	/// The comparer to use, or null to use the default comparer.
	/// </param>
	/// <returns>
	/// The zero-based index of the element, if found; otherwise, a negative number.
	/// </returns>
	public int BinarySearch(int index, int length, T value,
		IComparer<T> comparer);

	/// <summary>
	/// Searches the entire array for an element using the default comparer.
	/// </summary>
	/// <param name="value">The object to locate.</param>
	/// <returns>
	/// The zero-based index of the element, if found; otherwise, a negative number.
	/// </returns>
	public int BinarySearch(T value);

	/// <summary>
	/// Searches the entire array using a specified comparer.
	/// </summary>
	/// <param name="value">The object to locate.</param>
	/// <param name="comparer">
	/// The comparer to use, or null to use the default comparer.
	/// </param>
	/// <returns>
	/// The zero-based index of the element, if found; otherwise, a negative number.
	/// </returns>
	public int BinarySearch(T value, IComparer<T> comparer);

	/// <summary>
	/// Performs the specified action on each element of the array.
	/// </summary>
	/// <param name="action">The action to perform on each element.</param>
	public void ForEach(Action<T> action);

	/// <summary>
	/// Sorts the elements in the entire array using the default comparer.
	/// </summary>
	public void Sort();

	/// <summary>
	/// Sorts the elements using a specified comparer.
	/// </summary>
	/// <param name="comparer">
	/// The comparer to use, or null to use the default comparer.
	/// </param>
	public void Sort(IComparer<T> comparer);

	/// <summary>
	/// Sorts the elements using a specified comparison delegate.
	/// </summary>
	/// <param name="comparison">
	/// The comparison delegate to use for sorting.
	/// </param>
	public void Sort(Comparison<T> comparison);

	/// <summary>
	/// Sorts a range of elements in the array using the default comparer.
	/// </summary>
	/// <param name="index">The starting index of the range to sort.</param>
	/// <param name="length">The number of elements to sort.</param>
	public void Sort(int index, int length);

	/// <summary>
	/// Sorts a range of elements using a specified comparer.
	/// </summary>
	/// <param name="index">The starting index of the range to sort.</param>
	/// <param name="length">The number of elements to sort.</param>
	/// <param name="comparer">
	/// The comparer to use, or null to use the default comparer.
	/// </param>
	public void Sort(int index, int length, IComparer<T> comparer);

	/// <summary>
	/// Reverses the order of the elements in the entire array.
	/// </summary>
	public void Reverse();

	/// <summary>
	/// Reverses the order of the elements in a range.
	/// </summary>
	/// <param name="index">The starting index of the range to reverse.</param>
	/// <param name="length">The number of elements to reverse.</param>
	public void Reverse(int index, int length);

	/// <summary>
	/// Finds the last element that matches the conditions defined by the predicate.
	/// </summary>
	/// <param name="match">
	/// The predicate delegate that defines the conditions of the element to search for.
	/// </param>
	/// <returns>
	/// The last element that matches the conditions, if found; otherwise, default(T).
	/// </returns>
	public T FindLast(Predicate<T> match);

	/// <summary>
	/// Finds the first element that matches the conditions defined by the predicate.
	/// </summary>
	/// <param name="match">
	/// The predicate delegate that defines the conditions of the element to search for.
	/// </param>
	/// <returns>
	/// The first element that matches the conditions, if found; otherwise, default(T).
	/// </returns>
	public T Find(Predicate<T> match);


	/// <summary>
	/// Returns a read-only wrapper around the current array.
	/// </summary>
	/// <returns>A read-only reactive collection.</returns>
	public IReadOnlyReactiveCollection<T> AsReadOnly();


	/// <summary>
	/// Creates a reactive clone of the current array.
	/// </summary>
	/// <returns>A new reactive array that is a clone.</returns>
	public IReactiveArray<T> CloneAsReactive();

	/// <summary>
	/// Creates a shallow copy of the current array.
	/// </summary>
	/// <returns>An array that is a shallow copy of this array.</returns>
	public T[] Clone();

	//todo: think abous the same interface with list for this method
	/// <summary>
	/// Subscribes to item change notifications by index.
	/// </summary>
	/// <param name="onItemChangedByIndex">
	/// The action to perform when an item changes, with the item and index.
	/// </param>
	/// <returns>A value-type subscription token.</returns>
	public Subscription<(T, int)> SubscribeOnItemChangedByIndex(Action<(T, int)> onItemChangedByIndex);

	//todo: think abous the same interface with list for this method
	/// <summary>
	/// Unsubscribes from item change notifications by index.
	/// </summary>
	/// <param name="onItemChangedByIndex">
	/// The action that was previously subscribed.
	/// </param>
	public void UnsubscribeOnItemChangedByIndex(Action<(T, int)> onItemChangedByIndex);
}
}
#endif

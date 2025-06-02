using System;
using Azzazelloqq.MVVM.Source.ReactiveLibrary.Collections.Base;

namespace Azzazelloqq.MVVM.Source.ReactiveLibrary.Collections.Queue
{
/// <summary>
/// Represents a reactive queue that supports event notifications for enqueuing and dequeuing items.
/// Inherits from <see cref="IReactiveCollection{T}"/> and <see cref="ICloneable"/>.
/// </summary>
/// <typeparam name="T">The type of elements stored in the queue.</typeparam>
public interface IReactiveQueue<T> : IReactiveCollection<T>, ICloneable
{
    /// <summary>
    /// Removes and returns the object at the beginning of the queue.
    /// </summary>
    /// <returns>The object that is removed from the beginning of the queue.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
    public T Dequeue();
    
    /// <summary>
    /// Adds an object to the end of the queue.
    /// </summary>
    /// <param name="item">The object to add to the queue.</param>
    public void Enqueue(T item);
    
    /// <summary>
    /// Returns the object at the beginning of the queue without removing it.
    /// </summary>
    /// <returns>The object at the beginning of the queue.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
    public T Peek();
    
    /// <summary>
    /// Copies the elements of the queue to a new array.
    /// </summary>
    /// <returns>An array containing elements from the queue.</returns>
    public T[] ToArray();

    /// <summary>
    /// Attempts to remove and return the object at the beginning of the queue without throwing an exception if the queue is empty.
    /// </summary>
    /// <param name="result">
    /// When this method returns, contains the object removed from the queue, or the default value of <typeparamref name="T"/> 
    /// if the queue is empty.
    /// </param>
    /// <returns>
    /// <c>true</c> if an object was successfully removed 
    public bool TryDequeue(out T result);
    
    /// <summary>
    /// Attempts to return the object at the beginning of the queue without removing it or throwing an exception if the queue is empty.
    /// </summary>
    /// <param name="result">
    /// When this method returns, contains the object at the beginning of the queue, or the default value of <typeparamref name="T"/> 
    /// if the queue is empty.
    /// </param>
    /// <returns>
    /// <c>true</c> if an object was successfully returned from the queue; otherwise, <c>false</c>.
    /// </returns>
    public bool TryPeek(out T result);
}
}
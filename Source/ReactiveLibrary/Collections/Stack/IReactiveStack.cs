using System;

namespace Azzazelloqq.MVVM.ReactiveLibrary.Collections
{
/// <summary>
/// Represents a reactive stack that provides event notifications for operations such as pushing, popping, 
/// and peeking at elements in the stack. Inherits from <see cref="IReactiveCollection{T}"/>.
/// </summary>
/// <typeparam name="T">The type of elements stored in the stack.</typeparam>
public interface IReactiveStack<T> : IReactiveCollection<T>
{
    /// <summary>
    /// Returns the object at the top of the stack without removing it.
    /// </summary>
    /// <returns>The object at the top of the stack.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the stack is empty.</exception>
    public T Peek();
    
    /// <summary>
    /// Removes and returns the object at the top of the stack.
    /// </summary>
    /// <returns>The object removed from the top of the stack.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the stack is empty.</exception>
    public T Pop();
    
    /// <summary>
    /// Inserts an object at the top of the stack.
    /// </summary>
    /// <param name="item">The object to push onto the stack.</param>
    public void Push(T item);
    
    /// <summary>
    /// Copies the elements of the stack to a new array.
    /// </summary>
    /// <returns>An array containing elements from the stack.</returns>
    public T[] ToArray();
    
    /// <summary>
    /// Attempts to return the object at the top of the stack without removing it or throwing an exception if the stack is empty.
    /// </summary>
    /// <param name="result">
    /// When this method returns, contains the object at the top of the stack, or the default value of <typeparamref name="T"/> 
    /// if the stack is empty.
    /// </param>
    /// <returns>
    /// <c>true</c> if an object was successfully returned from the stack; otherwise, <c>false</c>.
    /// </returns>
    public bool TryPeek(out T result);

    /// <summary>
    /// Attempts to remove and return the object at the top of the stack without throwing an exception if the stack is empty.
    /// </summary>
    /// <param name="result">
    /// When this method returns, contains the object removed from the stack, or the default value of <typeparamref name="T"/> 
    /// if the stack is empty.
    /// </param>
    /// <returns>
    /// <c>true</c> if an object was successfully removed from the stack; otherwise, <c>false</c>.
    /// </returns>
    public bool TryPop(out T result);
}
}
﻿namespace MVVM.MVVM.ReactiveLibrary.Property
{
/// <summary>
/// Represents a reactive property that allows both reading and setting its value.
/// Inherits from <see cref="IReadOnlyReactiveProperty{TValue}"/>.
/// </summary>
/// <typeparam name="TValue">The type of the value stored in the property.</typeparam>
public interface IReactiveProperty<TValue> : IReadOnlyReactiveProperty<TValue>
{
    /// <summary>
    /// Sets the value of the property and notifies subscribers about the change.
    /// </summary>
    /// <param name="value">The new value to set.</param>
    public void SetValue(TValue value);
}
}
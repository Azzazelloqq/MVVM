#if !PROJECT_SUPPORT_R3
using System.Collections.Generic;

namespace Azzazelloqq.MVVM.ReactiveLibrary.Collections
{
/// <summary>
/// Represents a reactive collection that provides event notifications when items are added, removed, or changed.
/// Combines read-only reactive functionality with mutable collection operations.
/// Inherits from <see cref="IReadOnlyReactiveCollection{T}"/> and <see cref="ICollection{T}"/>.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
public interface IReactiveCollection<T> : IReadOnlyReactiveCollection<T>, ICollection<T>
{
}
}
#endif

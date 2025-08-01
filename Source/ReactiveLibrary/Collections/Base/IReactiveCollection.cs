using System.Collections.Generic;

namespace Azzazelloqq.MVVM.ReactiveLibrary.Collections
{
public interface IReactiveCollection<T> : IReadOnlyReactiveCollection<T>, ICollection<T>
{
}
}
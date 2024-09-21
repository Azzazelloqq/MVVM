using System.Collections.Generic;

namespace MVVM.MVVM.ReactiveLibrary.Collections.Base
{
public interface IReactiveCollection<T> : IReadOnlyReactiveCollection<T>, ICollection<T>
{
}
}
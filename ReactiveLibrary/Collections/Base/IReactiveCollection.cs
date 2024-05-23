using System.Collections.Generic;

namespace MVVM.ReactiveLibrary.Collections.Base
{
public interface IReactiveCollection<T> : IReadOnlyReactiveCollection<T> , ICollection<T>
{
}
}
using System.Collections.Generic;
using MVVM.MVVM.ReactiveLibrary.Collections.Base;

namespace MVVM.MVVM.ReactiveLibrary.Collections.Dictionary
{
public interface IReactiveDictionary<TKey, TValue> :
    IReactiveCollection<KeyValuePair<TKey, TValue>>,
    IDictionary<TKey, TValue>
{
}
}
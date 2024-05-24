using System;
using System.Collections.Generic;
using MVVM.MVVM.ReactiveLibrary.Collections.Base;

namespace MVVM.MVVM.ReactiveLibrary.Collections.List
{
public interface IReactiveList<T> : IReactiveCollection<T>, IList<T>
{
    public void SubscribeOnItemAddedByIndex(Action<T, int> onItemAdded);
    public void SubscribeOnItemRemovedByIndex(Action<T, int> onItemRemoved);
    
    public void UnsubscribeOnItemAddedByIndex(Action<T, int> onItemAdded);
    public void UnsubscribeOnItemRemovedByIndex(Action<T, int> onItemRemoved);
}
}
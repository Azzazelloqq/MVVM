using System;
using System.Collections.Generic;

namespace MVVM.ReactiveLibrary.Collections.Base
{
public interface IReadOnlyReactiveCollection<out T> : IEnumerable<T>, IDisposable
{
    public bool IsDisposed { get; }

    public void SubscribeOnItemAdded(Action<T> onItemAdded);
    public void SubscribeOnItemRemoved(Action<T> onItemRemoved);
    public void SubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved);
    public void SubscribeOnCollectionChanged(Action<IEnumerable<T>> collectionChanged, bool notifyOnSubscribe = true);
    
    public void UnsubscribeOnCollectionChanged(Action<IEnumerable<T>> collection);
    public void UnsubscribeOnCollectionChanged(Action<T> onItemAdded, Action<T> onItemRemoved);
    public void UnsubscribeOnItemAdded(Action<T> onItemAdded);
    public void UnsubscribeOnItemRemoved(Action<T> onItemRemoved);
}
}
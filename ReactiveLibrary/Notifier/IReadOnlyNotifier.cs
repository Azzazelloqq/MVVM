using System;
using MVVM.MVVM.ReactiveLibrary.Base;

namespace MVVM.MVVM.ReactiveLibrary.Notifier
{
public interface IReadOnlyNotifier : IReactive
{
    public void Subscribe(Action onNotify);
    public void Unsubscribe(Action onNotify);
}
}
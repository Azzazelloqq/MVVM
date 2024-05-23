using System;

namespace MVVM.ReactiveLibrary.Property
{
public interface IReadOnlyReactiveProperty<out TValue>
{
    public bool IsDisposed { get; }
    public bool HasValue { get; }
    public TValue Value { get; }

    public void Subscribe(Action<TValue> onValueChanged, bool withNotify = true);
    public void SubscribeOnce(Action<TValue> onValueChanged);
    public void Unsubscribe(Action<TValue> onValueChanged);
}
}
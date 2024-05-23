namespace MVVM.ReactiveLibrary.Property
{
public interface IReactiveProperty<TValue> : IReadOnlyReactiveProperty<TValue>
{
    public void SetValue(TValue value);
}
}
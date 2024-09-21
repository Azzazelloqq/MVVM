using MVVM.MVVM.ReactiveLibrary.Collections.Base;

namespace MVVM.MVVM.ReactiveLibrary.Collections.Stack
{
public interface IReactiveStack<T> : IReactiveCollection<T>
{
    public T Peek();
    public T Pop();
    public void Push(T item);
    public T[] ToArray();
    public bool TryPeek(out T result);
    public bool TryPop(out T result);
}
}
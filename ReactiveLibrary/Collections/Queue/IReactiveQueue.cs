using MVVM.MVVM.ReactiveLibrary.Collections.Base;

namespace MVVM.MVVM.ReactiveLibrary.Collections.Queue
{
public interface IReactiveQueue<T> : IReactiveCollection<T>, System.ICloneable
{
    public T Dequeue();
    public void Enqueue(T item);
    public T Peek();
    public T[] ToArray();
    public bool TryDequeue(out T result);
    public bool TryPeek(out T result);
}
}
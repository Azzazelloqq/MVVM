namespace MVVM.MVVM.System.Base.Command.Base
{
public interface IRelayCommand<in T> : ICommand<T>
{
    public void Execute(T parameter);
}
}
namespace MVVM.MVVM.System.Base.Command.Base
{
public interface ICommand<in T>
{
    public bool CanExecute(T parameter);
    public void Execute(T parameter);
}
}
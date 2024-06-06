namespace MVVM.MVVM.System.Base.Command.Base
{
public abstract class CommandBase<T> : ICommand<T>
{
    public virtual bool CanExecute(T parameter) => true;

    public abstract void Execute(T parameter);
}
}
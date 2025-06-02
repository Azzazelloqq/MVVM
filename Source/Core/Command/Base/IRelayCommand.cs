namespace Azzazelloqq.MVVM.Source.Core.Command.Base
{
public interface IRelayCommand<in T> : ICommand
{
    /// <summary>
    /// Executes the command with the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter of type <typeparamref name="T"/> to pass to the command.</param>
    public void Execute(T parameter);
}
}
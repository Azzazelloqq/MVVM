namespace MVVM.MVVM.System.Base.Command.Base
{
/// <summary>
/// Represents a command that can be executed with a parameter of type <typeparamref name="T"/>.
/// Inherits from <see cref="ICommand{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the parameter passed to the command when it is executed.</typeparam>
public interface IRelayCommand<in T> : ICommand<T>
{
    /// <summary>
    /// Executes the command with the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter of type <typeparamref name="T"/> to pass to the command.</param>
    public void Execute(T parameter);
}
}
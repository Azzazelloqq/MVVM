using System.Threading.Tasks;

namespace MVVM.MVVM.System.Base.Command.Base
{
/// <summary>
/// Represents an asynchronous command that can execute an operation with a parameter of type <typeparamref name="T"/>.
/// Inherits from <see cref="ICommand{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the parameter passed to the command when it is executed.</typeparam>
public interface IAsyncCommand<in T> : ICommand<T>
{
    /// <summary>
    /// Executes the command asynchronously with the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter of type <typeparamref name="T"/> to pass to the command.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task ExecuteAsync(T parameter);
}
}
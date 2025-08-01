using System.Threading.Tasks;

namespace Azzazelloqq.MVVM.Core
{
/// <summary>
/// Represents an asynchronous command that can be executed with a parameter of type <typeparamref name="T"/>.
/// </summary>
public interface IAsyncCommand<in T> : ICommand
{
    /// <summary>
    /// Executes the command asynchronously with the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter of type <typeparamref name="T"/> to pass to the command.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task ExecuteAsync(T parameter);
}
}
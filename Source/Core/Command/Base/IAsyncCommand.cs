#if PROJECT_SUPPORT_UNITASK
using Cysharp.Threading.Tasks;
using MVVMTask = Cysharp.Threading.Tasks.UniTask;
#else
using System.Threading.Tasks;
using MVVMTask = System.Threading.Tasks.Task;
#endif

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
    /// <returns>An awaitable representing the asynchronous operation.</returns>
    public MVVMTask ExecuteAsync(T parameter);
}
}
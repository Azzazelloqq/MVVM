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
/// Represents a simple asynchronous command that can be executed.
/// </summary>
public interface IActionAsyncCommand : ICommand
{
	/// <summary>
	/// Executes the command asynchronously.
	/// </summary>
	public MVVMTask ExecuteAsync();
}
}
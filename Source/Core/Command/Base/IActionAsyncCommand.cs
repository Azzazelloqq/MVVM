using System.Threading.Tasks;

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
	public Task ExecuteAsync();
}
}
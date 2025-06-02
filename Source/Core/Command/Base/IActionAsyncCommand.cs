using System.Threading.Tasks;

namespace Azzazelloqq.MVVM.Source.Core.Command.Base
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
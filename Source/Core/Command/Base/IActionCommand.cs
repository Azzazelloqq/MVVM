namespace Azzazelloqq.MVVM.Source.Core.Command.Base
{
/// <summary>
/// Represents a simple command that can be executed.
/// </summary>
public interface IActionCommand : ICommand
{
	/// <summary>
	/// Executes the command.
	/// </summary>
	public void Execute();
}
}
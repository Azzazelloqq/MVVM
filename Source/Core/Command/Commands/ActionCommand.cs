using System;
namespace Azzazelloqq.MVVM.Core
{
/// <summary>
/// Represents a command that can be executed with an action.
/// </summary>
public class ActionCommand : IActionCommand
{
	private Action _execute;
	private readonly Func<bool> _canExecuteEvaluator;
	private bool _isDisposed;

	/// <summary>
	/// Initializes a new instance of the <see cref="ActionCommand"/> class.
	/// </summary>
	/// <param name="execute">The action to execute.</param>
	/// <param name="canExecute">The function that determines whether the command can execute. If null, the command can always execute.</param>
	/// <exception cref="ArgumentNullException">Thrown if the execute action is null.</exception>
	public ActionCommand(Action execute, Func<bool> canExecute = null)
	{
		_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		_canExecuteEvaluator = canExecute ?? (() => true);
	}

	public void Dispose()
	{
		_isDisposed = true;
		_execute = null;
	}

	/// <inheritdoc/>
	public bool CanExecute()
	{
		if (_isDisposed)
		{
			return false;
		}

		return _canExecuteEvaluator();
	}

	/// <inheritdoc/>
	public void Execute()
	{
		if (!CanExecute())
		{
			return;
		}
        
		_execute?.Invoke();
	}
}
}
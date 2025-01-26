using System;
using MVVM.MVVM.ReactiveLibrary.Property;
using MVVM.MVVM.System.Base.Command.Base;

namespace MVVM.MVVM.System.Base.Command.Commands
{
/// <summary>
/// Represents a command that can be executed with an action.
/// </summary>
public class ActionCommand : IActionCommand
{
	private Action _execute;
	private readonly ReactiveProperty<bool> _canExecute;

	/// <summary>
	/// Initializes a new instance of the <see cref="ActionCommand"/> class.
	/// </summary>
	/// <param name="execute">The action to execute.</param>
	/// <param name="canExecute">The function that determines whether the command can execute. If null, the command can always execute.</param>
	/// <exception cref="ArgumentNullException">Thrown if the execute action is null.</exception>
	public ActionCommand(Action execute, Func<bool> canExecute = null)
	{
		_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		_canExecute = new ReactiveProperty<bool>(canExecute?.Invoke() ?? true);
	}

	public void Dispose()
	{
		_canExecute.Dispose();
		_execute = null;
	}

	/// <inheritdoc/>
	public bool CanExecute()
	{
		return _canExecute.Value;
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
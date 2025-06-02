using System;
using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.Source.Core.Command.Base;
using Azzazelloqq.MVVM.Source.ReactiveLibrary.Property;

namespace Azzazelloqq.MVVM.Source.Core.Command.Commands
{
/// <summary>
/// Represents an asynchronous command that can be executed with an action.
/// </summary>
public class ActionAsyncCommand : IActionAsyncCommand
{
	private Func<Task> _execute;
	private readonly ReactiveProperty<bool> _canExecute;
	private readonly CancellationTokenSource _disposeCancellationTokenSource;

	/// <summary>
	/// Initializes a new instance of the <see cref="ActionAsyncCommand"/> class.
	/// </summary>
	/// <param name="execute">The function to execute asynchronously.</param>
	/// <param name="canExecute">The function that determines whether the command can execute. If null, the command can always execute.</param>
	/// <exception cref="ArgumentNullException">Thrown if the execute function is null.</exception>
	public ActionAsyncCommand(Func<Task> execute, Func<bool> canExecute = null)
	{
		_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		_canExecute = new ReactiveProperty<bool>(canExecute?.Invoke() ?? true);
		_disposeCancellationTokenSource = new CancellationTokenSource();
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		_execute = null;
		_canExecute.Dispose();
		_disposeCancellationTokenSource.Dispose();
	}

	/// <inheritdoc/>
	public async Task ExecuteAsync()
	{
		if (!CanExecute())
		{
			return;
		}

		if (_disposeCancellationTokenSource.IsCancellationRequested)
		{
			return;
		}
        
		await _execute();
	}

	/// <inheritdoc/>
	public bool CanExecute()
	{
		var isCancellationRequested = _disposeCancellationTokenSource.IsCancellationRequested;
        
		return _canExecute.Value && !isCancellationRequested;
	}
}
}
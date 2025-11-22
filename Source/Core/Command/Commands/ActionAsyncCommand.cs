using System;
using System.Threading;
using System.Threading.Tasks;
namespace Azzazelloqq.MVVM.Core
{
/// <summary>
/// Represents an asynchronous command that can be executed with an action.
/// </summary>
public class ActionAsyncCommand : IActionAsyncCommand
{
	private Func<Task> _execute;
	private readonly Func<bool> _canExecuteEvaluator;
	private readonly CancellationTokenSource _disposeCancellationTokenSource;
	private bool _isDisposed;

	/// <summary>
	/// Initializes a new instance of the <see cref="ActionAsyncCommand"/> class.
	/// </summary>
	/// <param name="execute">The function to execute asynchronously.</param>
	/// <param name="canExecute">The function that determines whether the command can execute. If null, the command can always execute.</param>
	/// <exception cref="ArgumentNullException">Thrown if the execute function is null.</exception>
	public ActionAsyncCommand(Func<Task> execute, Func<bool> canExecute = null)
	{
		_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		_canExecuteEvaluator = canExecute ?? (() => true);
		_disposeCancellationTokenSource = new CancellationTokenSource();
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		if (_isDisposed)
		{
			return;
		}

		_isDisposed = true;
		_execute = null;
		_disposeCancellationTokenSource.Dispose();
	}

	/// <inheritdoc/>
	public async Task ExecuteAsync()
	{
		if (_isDisposed)
		{
			return;
		}

		if (!CanExecute())
		{
			return;
		}

		if (_isDisposed || _disposeCancellationTokenSource.IsCancellationRequested)
		{
			return;
		}
        
		await _execute();
	}

	/// <inheritdoc/>
	public bool CanExecute()
	{
		if (_isDisposed)
		{
			return false;
		}

		if (_disposeCancellationTokenSource.IsCancellationRequested)
		{
			return false;
		}

		return _canExecuteEvaluator();
	}
}
}
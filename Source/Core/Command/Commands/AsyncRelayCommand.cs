using System;
using System.Threading;
using System.Threading.Tasks;
namespace Azzazelloqq.MVVM.Core
{
/// <summary>
/// Represents an asynchronous reactive command that can execute an operation asynchronously 
/// with a parameter of type <typeparamref name="T"/>.
/// Implements <see cref="IAsyncCommand{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the parameter passed to the command when it is executed.</typeparam>
public class AsyncRelayCommand<T> : IAsyncCommand<T>
{
    private Func<T, Task> _execute;
    private readonly Func<bool> _canExecuteEvaluator;
    private readonly CancellationTokenSource _disposeCancellationTokenSource;
    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncRelayCommand{T}"/> class.
    /// </summary>
    /// <param name="execute">The asynchronous function to execute when the command is triggered.</param>
    /// <param name="canExecute">A function that determines whether the command can be executed. Defaults to <c>true</c> if not provided.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="execute"/> function is <c>null</c>.</exception>
    public AsyncRelayCommand(Func<T, Task> execute, Func<bool> canExecute = null)
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
    public async Task ExecuteAsync(T parameter)
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
        
        await _execute(parameter);
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
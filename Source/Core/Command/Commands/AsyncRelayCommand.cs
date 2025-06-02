using System;
using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.Source.Core.Command.Base;
using Azzazelloqq.MVVM.Source.ReactiveLibrary.Property;

namespace Azzazelloqq.MVVM.Source.Core.Command.Commands
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
    private readonly ReactiveProperty<bool> _canExecute;
    private readonly CancellationTokenSource _disposeCancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncRelayCommand{T}"/> class.
    /// </summary>
    /// <param name="execute">The asynchronous function to execute when the command is triggered.</param>
    /// <param name="canExecute">A function that determines whether the command can be executed. Defaults to <c>true</c> if not provided.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="execute"/> function is <c>null</c>.</exception>
    public AsyncRelayCommand(Func<T, Task> execute, Func<bool> canExecute = null)
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
    public async Task ExecuteAsync(T parameter)
    {
        if (!CanExecute())
        {
            return;
        }

        if (_disposeCancellationTokenSource.IsCancellationRequested)
        {
            return;
        }
        
        await _execute(parameter);
    }

    /// <inheritdoc/>
    public bool CanExecute()
    {
        var isCancellationRequested = _disposeCancellationTokenSource.IsCancellationRequested;
        
        return _canExecute.Value && !isCancellationRequested;
    }
}
}
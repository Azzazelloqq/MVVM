using System;
namespace Azzazelloqq.MVVM.Core
{
/// <summary>
/// Represents a reactive command that can execute an action with a parameter of type <typeparamref name="T"/>.
/// Implements <see cref="IRelayCommand{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the parameter passed to the command when it is executed.</typeparam>
public class RelayCommand<T> : IRelayCommand<T>
{
    private Action<T> _execute;
    private readonly Func<bool> _canExecuteEvaluator;
    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
    /// </summary>
    /// <param name="execute">The action to execute when the command is triggered.</param>
    /// <param name="canExecute">A function that determines whether the command can be executed. Defaults to <c>true</c> if not provided.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="execute"/> action is <c>null</c>.</exception>
    public RelayCommand(Action<T> execute, Func<bool> canExecute = null)
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
    public void Execute(T parameter)
    {
        if (!CanExecute())
        {
            return;
        }
        
        _execute?.Invoke(parameter);
    }
}
}
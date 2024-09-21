using System;
using MVVM.MVVM.ReactiveLibrary.Property;
using MVVM.MVVM.System.Base.Command.Base;

namespace MVVM.MVVM.System.Base.Command.Commands
{
/// <summary>
/// Represents a reactive command that can execute an action with a parameter of type <typeparamref name="T"/>.
/// Implements <see cref="IRelayCommand{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the parameter passed to the command when it is executed.</typeparam>
public class RelayCommand<T> : IRelayCommand<T>
{
    private Action<T> _execute;
    private readonly ReactiveProperty<bool> _canExecute;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
    /// </summary>
    /// <param name="execute">The action to execute when the command is triggered.</param>
    /// <param name="canExecute">A function that determines whether the command can be executed. Defaults to <c>true</c> if not provided.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="execute"/> action is <c>null</c>.</exception>
    public RelayCommand(Action<T> execute, Func<bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = new ReactiveProperty<bool>(canExecute?.Invoke() ?? true);
    }
    
    /// <inheritdoc/>
    public bool CanExecute()
    {
        return _canExecute.Value;
    }

    public void Dispose()
    {
        _canExecute.Dispose();
        _execute = null;
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
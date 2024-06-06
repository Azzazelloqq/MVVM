using System;
using MVVM.MVVM.System.Base.Command.Base;

namespace MVVM.MVVM.System.Base.Command.Commands
{
public class RelayCommand<T> : CommandBase<T>
{
    private readonly Action<T> _execute;
    private readonly Func<T, bool> _canExecute;

    public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }
    
    public override bool CanExecute(T parameter) => _canExecute?.Invoke(parameter) ?? true;

    public override void Execute(T parameter) => _execute(parameter);
}
}
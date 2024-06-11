using System;
using MVVM.MVVM.ReactiveLibrary.Property;
using MVVM.MVVM.System.Base.Command.Base;

namespace MVVM.MVVM.System.Base.Command.Commands
{
public class ReactiveCommand<T> : IRelayCommand<T>
{
    private readonly Action<T> _execute;
    private readonly ReactiveProperty<bool> _canExecute;

    public ReactiveCommand(Action<T> execute, Func<bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = new ReactiveProperty<bool>(canExecute?.Invoke() ?? true);
    }
    
    public bool CanExecute() => _canExecute.Value;

    public void Execute(T parameter)
    {
        if (!CanExecute())
        {
            return;
        }
        
        _execute(parameter);
    }
}
}
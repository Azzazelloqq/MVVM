using System;
using System.Threading.Tasks;
using MVVM.MVVM.ReactiveLibrary.Property;
using MVVM.MVVM.System.Base.Command.Base;

namespace MVVM.MVVM.System.Base.Command.Commands
{
public class AsyncReactiveCommand<T> : IAsyncCommand<T>
{
    private readonly Func<Task> _execute;
    private readonly ReactiveProperty<bool> _canExecute;

    public AsyncReactiveCommand(Func<Task> execute, Func<bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = new ReactiveProperty<bool>(canExecute?.Invoke() ?? true);
    }

    public async Task ExecuteAsync(T parameter)
    {
        if (CanExecute())
        {
            await _execute();
        }
    }

    public bool CanExecute()
    {
        return _canExecute.Value;
    }
}
}
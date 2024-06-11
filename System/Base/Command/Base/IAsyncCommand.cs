using System.Threading.Tasks;

namespace MVVM.MVVM.System.Base.Command.Base
{
public interface IAsyncCommand<in T> : ICommand<T>
{
    public Task ExecuteAsync(T parameter);
}
}
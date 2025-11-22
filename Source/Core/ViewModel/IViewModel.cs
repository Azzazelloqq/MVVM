using Azzazelloqq.MVVM.ReactiveLibrary;

namespace Azzazelloqq.MVVM.Core
{
/// <summary>
/// Represents a basic view model in the MVVM architecture. 
/// Serves as a marker interface for all view models.
/// </summary>
public interface IViewModel
{
    public IReadOnlyNotifier DisposeNotifier { get; }
    public IReadOnlyReactiveProperty<bool> IsInitialized { get; }
}
}
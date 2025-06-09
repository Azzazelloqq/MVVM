using Azzazelloqq.MVVM.Source.ReactiveLibrary.Notifier;

namespace Azzazelloqq.MVVM.Source.Core.ViewModel
{
/// <summary>
/// Represents a basic view model in the MVVM architecture. 
/// Serves as a marker interface for all view models.
/// </summary>
public interface IViewModel
{
    public IReadOnlyNotifier DisposeNotifier { get; }
}
}
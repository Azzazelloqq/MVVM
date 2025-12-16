#if PROJECT_SUPPORT_R3
using R3;
#else
using Azzazelloqq.MVVM.ReactiveLibrary;
#endif

namespace Azzazelloqq.MVVM.Core
{
/// <summary>
/// Represents a basic view model in the MVVM architecture. 
/// Serves as a marker interface for all view models.
/// </summary>
public interface IViewModel
{
	#if PROJECT_SUPPORT_R3
    public Observable<Unit> DisposeNotifier { get; }
    public ReadOnlyReactiveProperty<bool> IsInitialized { get; }
	#else
    public IReadOnlyNotifier DisposeNotifier { get; }
    public IReadOnlyReactiveProperty<bool> IsInitialized { get; }
	#endif
}
}

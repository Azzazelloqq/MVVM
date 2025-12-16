using System;
using System.Threading;
#if PROJECT_SUPPORT_UNITASK
using Cysharp.Threading.Tasks;
using MVVMTask = Cysharp.Threading.Tasks.UniTask;
#else
using System.Threading.Tasks;
using MVVMTask = System.Threading.Tasks.Task;
#endif
#if PROJECT_SUPPORT_R3
using R3;
#else
using Azzazelloqq.MVVM.ReactiveLibrary;
#endif

namespace Azzazelloqq.MVVM.Core
{
/// <summary>
/// Represents a basic view in the MVVM architecture. Serves as a marker interface for all views.
/// </summary>
public interface IView : IDisposable
{
#if PROJECT_SUPPORT_R3
	public ReadOnlyReactiveProperty<bool> IsInitialized { get; }
#else
	public IReadOnlyReactiveProperty<bool> IsInitialized { get; }
#endif
	
	/// <summary>
	/// Asynchronously initializes the view. This method must be called after the model is created 
	/// to set up any necessary state or dependencies. Failure to call this method may result in incorrect behavior.
	/// </summary>
	/// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete. 
	/// It allows the operation to be canceled.</param>
	/// <returns>An awaitable that represents the asynchronous initialization operation.</returns>
	public MVVMTask InitializeAsync(IViewModel viewModel, CancellationToken token, bool disposeWithViewModel = true);

	/// <summary>
	/// Initializes the view. This method must be called after the model is created to set up any necessary state or dependencies.
	/// Failure to call this method may result in incorrect behavior.
	/// </summary>
	public void Initialize(IViewModel viewModel, bool disposeWithViewModel = true);
}
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.ReactiveLibrary;

namespace Azzazelloqq.MVVM.Core
{
/// <summary>
/// Represents a basic view in the MVVM architecture. Serves as a marker interface for all views.
/// </summary>
public interface IView : IDisposable
{
	public IReadOnlyReactiveProperty<bool> IsInitialized { get; }
	
	/// <summary>
	/// Asynchronously initializes the view. This method must be called after the model is created 
	/// to set up any necessary state or dependencies. Failure to call this method may result in incorrect behavior.
	/// </summary>
	/// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete. 
	/// It allows the operation to be canceled.</param>
	/// <returns>A task that represents the asynchronous initialization operation.</returns>
	public Task InitializeAsync(IViewModel viewModel, CancellationToken token, bool disposeWithViewModel = true);

	/// <summary>
	/// Initializes the view. This method must be called after the model is created to set up any necessary state or dependencies.
	/// Failure to call this method may result in incorrect behavior.
	/// </summary>
	public void Initialize(IViewModel viewModel, bool disposeWithViewModel = true);
}
}
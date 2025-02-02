using System;
using System.Threading;
using System.Threading.Tasks;
using MVVM.MVVM.System.Base.ViewModel;

namespace MVVM.MVVM.System.Base.View
{
/// <summary>
/// Represents a basic view in the MVVM architecture. Serves as a marker interface for all views.
/// </summary>
public interface IView : IDisposable
{
	/// <summary>
	/// Asynchronously initializes the view. This method must be called after the model is created 
	/// to set up any necessary state or dependencies. Failure to call this method may result in incorrect behavior.
	/// </summary>
	/// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete. 
	/// It allows the operation to be canceled.</param>
	/// <returns>A task that represents the asynchronous initialization operation.</returns>
	public Task InitializeAsync(IViewModel viewModel, CancellationToken token);

	/// <summary>
	/// Initializes the view. This method must be called after the model is created to set up any necessary state or dependencies.
	/// Failure to call this method may result in incorrect behavior.
	/// </summary>
	public void Initialize(IViewModel viewModel);
}
}
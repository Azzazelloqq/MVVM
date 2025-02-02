using System;
using System.Threading;
using System.Threading.Tasks;

namespace MVVM.MVVM.System.Base.Model
{
/// <summary>
/// Represents a basic model in the MVVM architecture. Serves as a marker interface for all models.
/// </summary>
public interface IModel : IDisposable
{
	/// <summary>
	/// Initializes the model. This method must be called after the model is created to set up any necessary state or dependencies.
	/// Failure to call this method may result in incorrect behavior.
	/// </summary>
	public void Initialize();

	/// <summary>
	/// Asynchronously initializes the model. This method must be called after the model is created 
	/// to set up any necessary state or dependencies. Failure to call this method may result in incorrect behavior.
	/// </summary>
	/// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete. 
	/// It allows the operation to be canceled.</param>
	/// <returns>A task that represents the asynchronous initialization operation.</returns>
	public Task InitializeAsync(CancellationToken token);
}
}
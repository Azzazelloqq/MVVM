using System.Threading;
using System.Threading.Tasks;
using Disposable;

namespace MVVM.MVVM.System.Base.Model
{
/// <summary>
/// Represents the base class for models in the MVVM architecture.
/// Implements <see cref="IModel"/> and extends <see cref="DisposableBase"/> to provide disposal functionality.
/// </summary>
public abstract class ModelBase : DisposableBase, IModel
{
	/// <summary>
	/// Initializes the model. This method must be called after the model is created to set up any necessary state or dependencies.
	/// Failure to call this method may result in incorrect behavior.
	/// </summary>
	public void Initialize()
	{
	}

	/// <summary>
	/// Asynchronously initializes the model. This method must be called after the model is created 
	/// to set up any necessary state or dependencies. Failure to call this method may result in incorrect behavior.
	/// </summary>
	/// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete. 
	/// It allows the operation to be canceled.</param>
	/// <returns>A task that represents the asynchronous initialization operation.</returns>
	public async Task InitializeAsync(CancellationToken token)
	{
		await OnInitializeAsync(token);
	}

	/// <summary>
	/// Provides a hook for subclasses to perform custom initialization logic.
	/// This method is called by the <see cref="Initialize"/> method.
	/// </summary>
	protected virtual void OnInitialize()
	{
	}

	/// <summary>
	/// Provides a hook for subclasses to perform custom asynchronous initialization logic.
	/// This method is called by the <see cref="InitializeAsync(CancellationToken)"/> method and can be overridden 
	/// in derived classes to implement specific asynchronous initialization behavior.
	/// </summary>
	/// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete. 
	/// It allows the operation to be canceled.</param>
	/// <returns>A task that represents the asynchronous initialization operation.</returns>
	protected virtual Task OnInitializeAsync(CancellationToken token)
	{
		return Task.CompletedTask;
	}
}
}
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
	/// A composite disposable that manages the disposal of both the view and the model, along with other disposable resources.
	/// </summary>
	protected readonly ICompositeDisposable compositeDisposable = new CompositeDisposable();
	
	/// <summary>
	/// Gets the cancellation token that is triggered when the model is disposed.
	/// </summary>
	protected CancellationToken disposeToken => _disposeCancellationSource.Token;
	
	/// <summary>
	/// The cancellation token source that is used to signal disposal of the model.
	/// </summary>
	private readonly CancellationTokenSource _disposeCancellationSource = new();
	
	/// <inheritdoc/>
	void IModel.Initialize()
	{
		OnInitialize();
	}

	/// <inheritdoc/>
	async Task IModel.InitializeAsync(CancellationToken token)
	{
		await OnInitializeAsync(token);
	}

	/// <summary>
	/// Provides a hook for subclasses to perform custom asynchronous initialization logic.
	/// This method is called by the <see cref="InitializeAsync(CancellationToken)"/> method and can be overridden 
	/// in derived classes to implement specific asynchronous initialization behavior.
	/// </summary>
	/// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete. 
	/// It allows the operation to be canceled.</param>
	/// <returns>A task that represents the asynchronous initialization operation.</returns>
	protected virtual async Task OnInitializeAsync(CancellationToken token)
	{
		await Task.CompletedTask;
		
		OnInitialize();
	}
	
	/// <summary>
	/// Provides a hook for subclasses to perform custom initialization logic.
	/// This method is called by the <see cref="Initialize"/> method.
	/// </summary>
	protected virtual void OnInitialize()
	{
	}
	
	/// <summary>
	/// Disposes of the resources used by the object.
	/// This method should be called when the object is no longer needed,
	/// and it will automatically call <see cref="OnDispose"/> for additional cleanup logic in derived classes.
	/// </summary>
	public sealed override void Dispose()
	{
		OnDispose();

		if (!_disposeCancellationSource.IsCancellationRequested)
		{
			_disposeCancellationSource.Cancel();
		}
		
		_disposeCancellationSource.Dispose();
		
		compositeDisposable.Dispose();
        
		base.Dispose();
	}

	/// <summary>
	/// Provides additional dispose logic for derived classes.
	/// Subclasses can override this method to implement custom cleanup code
	/// without overriding the base <see cref="Dispose"/> method.
	/// </summary>
	protected virtual void OnDispose()
	{
		// Subclasses can override this method to perform custom dispose logic.
	}
}
}
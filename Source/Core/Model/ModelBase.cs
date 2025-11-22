using System;
using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.ReactiveLibrary;
using Disposable;

namespace Azzazelloqq.MVVM.Core
{
/// <summary>
/// Represents the base class for models in the MVVM architecture.
/// Implements <see cref="IModel"/> and extends <see cref="DisposableBase"/> to provide disposal functionality.
/// </summary>
public abstract class ModelBase : DisposableBase, IModel
{
	public IReadOnlyReactiveProperty<bool> IsInitialized => _isInitialized;
	
	/// <summary>
	/// A composite disposable that manages the disposal of both the view and the model, along with other disposable resources.
	/// </summary>
	protected readonly ICompositeDisposable compositeDisposable = new CompositeDisposable();
	
	/// <summary>
	/// Gets the cancellation token that is triggered when the model is disposed.
	/// </summary>
	protected CancellationToken disposeToken => disposeCancellationToken;

	private ReactiveProperty<bool> _isInitialized = new();

	/// <inheritdoc/>
	void IModel.Initialize()
	{
		if (_isInitialized.Value)
		{
			throw new Exception("Model has already been initialized.");
		}

		OnInitialize();

		_isInitialized.SetValue(true);
	}

	/// <inheritdoc/>
	async Task IModel.InitializeAsync(CancellationToken token)
	{
		if (_isInitialized.Value)
		{
			throw new Exception("Model has already been initialized.");
		}
		
		await OnInitializeAsync(token);
		
		_isInitialized.SetValue(true);
	}
	
	/// <summary>
	/// Disposes of the resources used by the object.
	/// This method should be called when the object is no longer needed,
	/// and it will automatically call <see cref="OnDispose"/> for additional cleanup logic in derived classes.
	/// </summary>
	protected sealed override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		
		OnDispose();
		
		compositeDisposable.Dispose();
	}
	
	public sealed override async ValueTask DisposeAsync(CancellationToken token, bool continueOnCapturedContext = false)
	{
		await base.DisposeAsync(token, continueOnCapturedContext);
		
		await OnDisposeAsync(token);
		
		compositeDisposable?.DisposeAsync(token);
	}

	/// <summary>
	/// Provides a hook for subclasses to perform custom asynchronous initialization logic.
	/// This method is called by the <see cref="InitializeAsync(CancellationToken)"/> method and can be overridden 
	/// in derived classes to implement specific asynchronous initialization behavior.
	/// </summary>
	/// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete. 
	/// It allows the operation to be canceled.</param>
	/// <returns>A task that represents the asynchronous initialization operation.</returns>
	protected abstract ValueTask OnInitializeAsync(CancellationToken token);

	/// <summary>
	/// Provides a hook for subclasses to perform custom initialization logic.
	/// This method is called by the <see cref="Initialize"/> method.
	/// </summary>
	protected abstract void OnInitialize();
	
	/// <summary>
	/// Provides additional dispose logic for derived classes.
	/// Subclasses can override this method to implement custom cleanup code
	/// without overriding the base <see cref="Dispose"/> method.
	/// </summary>
	protected abstract void OnDispose();
	protected abstract ValueTask OnDisposeAsync(CancellationToken token);
}
}
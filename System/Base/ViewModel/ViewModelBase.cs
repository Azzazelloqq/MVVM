using System.Threading;
using System.Threading.Tasks;
using Disposable;
using MVVM.MVVM.System.Base.Model;
using MVVM.MVVM.System.Base.View;

namespace MVVM.MVVM.System.Base.ViewModel
{
/// <summary>
/// Represents the base class for view models in the MVVM architecture.
/// Implements <see cref="IViewModel"/> and provides support for a model of type <typeparamref name="TModel"/>.
/// Inherits from <see cref="DisposableBase"/> to provide disposal functionality.
/// </summary>
/// <typeparam name="TModel">The type of the model associated with the view model, which implements <see cref="IModel"/>.</typeparam>
public abstract class ViewModelBase<TModel> : DisposableBase, IViewModel where TModel : IModel
{
    /// <summary>
    /// The model associated with the view model.
    /// </summary>
    protected TModel model;

    /// <summary>
    /// A composite disposable that manages the disposal of both the view and the model, along with other disposable resources.
    /// </summary>
    protected readonly ICompositeDisposable compositeDisposable = new CompositeDisposable();
    
    /// <summary>
    /// Gets the cancellation token that is triggered when the viewModel is disposed.
    /// </summary>
    protected CancellationToken disposeToken => _disposeCancellationSource.Token;
	
    /// <summary>
    /// The cancellation token source that is used to signal disposal of the viewModel.
    /// </summary>
    private readonly CancellationTokenSource _disposeCancellationSource = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModelBase{TModel}"/> class with the specified model.
    /// </summary>
    /// <param name="model">The model to associate with the view model.</param>
    public ViewModelBase(TModel model)
    {
        this.model = model;
        
        compositeDisposable.AddDisposable(model);
    }

    /// <summary>
    /// Asynchronously initializes the view model. This method must be called after the view model is created 
    /// to set up any necessary state or dependencies. Failure to call this method may result in incorrect behavior.
    /// It calls the <see cref="OnInitializeAsync(CancellationToken)"/> method to allow subclasses to perform custom asynchronous initialization logic.
    /// </summary>
    /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete. 
    /// It allows the operation to be canceled.</param>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    public async Task InitializeAsync(CancellationToken token)
    {
        await model.InitializeAsync(token);
        
        await OnInitializeAsync(token);
    }
    
    /// <summary>
    /// Initializes the view with the specified view model.
    /// </summary>
    /// <param name="viewModel">The view model to associate with the view.</param>
    public void Initialize()
    {
        model.Initialize();
        
        OnInitialize();
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
    /// Provides a hook for subclasses to perform custom initialization logic.
    /// This method is called by the <see cref="Initialize"/> method.
    /// </summary>
    protected virtual void OnInitialize()
    {
        // Subclasses can override this method to perform custom initialization logic.
    }
    
    /// <summary>
    /// Provides a hook for subclasses to perform custom asynchronous initialization logic.
    /// This method is called by the <see cref="InitializeAsync(CancellationToken)"/> method.
    /// Subclasses can override this method to implement specific asynchronous initialization behavior.
    /// </summary>
    /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete. 
    /// It allows the operation to be canceled.</param>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    protected virtual Task OnInitializeAsync(CancellationToken token)
    {
        return Task.CompletedTask;
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
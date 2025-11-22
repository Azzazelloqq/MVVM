using System;
using System.Threading;
using System.Threading.Tasks;
using Azzazelloqq.MVVM.ReactiveLibrary;
using Disposable;

namespace Azzazelloqq.MVVM.Core
{
/// <summary>
/// Represents the base class for view models in the MVVM architecture.
/// Implements <see cref="IViewModel"/> and provides support for a model of type <typeparamref name="TModel"/>.
/// Inherits from <see cref="DisposableBase"/> to provide disposal functionality.
/// </summary>
/// <typeparam name="TModel">The type of the model associated with the view model, which implements <see cref="IModel"/>.</typeparam>
public abstract class ViewModelBase<TModel> : DisposableBase, IViewModel where TModel : IModel
{
    public IReadOnlyNotifier DisposeNotifier => _disposeNotifier;
    public IReadOnlyReactiveProperty<bool> IsInitialized => _isInitialized;
    
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
    protected CancellationToken disposeToken => disposeCancellationToken;

    private readonly ReactiveNotifier _disposeNotifier = new();
    private readonly ReactiveProperty<bool> _isInitialized = new();

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
        if (_isInitialized.Value)
        {
            throw new Exception("ViewModel has already been initialized.");
        }
        
        await model.InitializeAsync(token);
        
        await OnInitializeAsync(token);

        _isInitialized.SetValue(true);
    }

    /// <summary>
    /// Initializes the view with the specified view model.
    /// </summary>
    public void Initialize()
    {
        if (_isInitialized.Value)
        {
            throw new Exception("ViewModel has already been initialized.");
        }
        
        model.Initialize();
        
        OnInitialize();
        
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
        
        _disposeNotifier.Notify();
        _disposeNotifier.Dispose();
        
    }

    public sealed override async ValueTask DisposeAsync(CancellationToken token, bool continueOnCapturedContext = false)
    {
        await base.DisposeAsync(token, continueOnCapturedContext);
        
        await OnDisposeAsync(token);
        
        await compositeDisposable.DisposeAsync(token);
        
        _disposeNotifier.Notify();
        _disposeNotifier.Dispose();
    }

    /// <summary>
    /// Adds a disposable resource to the collection of disposables managed by the view model.
    /// </summary>
    /// <param name="disposable">The disposable resource to be added to the composite disposable managed by the view model.</param>
    protected void AddDisposable(IDisposable disposable)
    {
        compositeDisposable.AddDisposable(disposable);
    }

    /// <summary>
    /// Adds an asynchronous disposable resource to the composite disposable collection of the view model.
    /// </summary>
    /// <param name="disposable">The asynchronous disposable resource to add to the collection.</param>
    protected void AddDisposable(IAsyncDisposable disposable)
    {
        compositeDisposable.AddDisposable(disposable);
    }

    
    /// <summary>
    /// Provides a hook for subclasses to perform custom initialization logic.
    /// This method is called by the <see cref="Initialize"/> method.
    /// </summary>
    protected abstract void OnInitialize();

    /// <summary>
    /// Provides a hook for subclasses to perform custom asynchronous initialization logic.
    /// This method is called by the <see cref="InitializeAsync(CancellationToken)"/> method.
    /// Subclasses can override this method to implement specific asynchronous initialization behavior.
    /// </summary>
    /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete. 
    /// It allows the operation to be canceled.</param>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    protected abstract ValueTask OnInitializeAsync(CancellationToken token);

    /// <summary>
    /// Provides additional dispose logic for derived classes.
    /// Subclasses can override this method to implement custom cleanup code
    /// without overriding the base <see cref="Dispose"/> method.
    /// </summary>
    protected abstract void OnDispose();
    
    protected abstract ValueTask OnDisposeAsync(CancellationToken token);
}
}
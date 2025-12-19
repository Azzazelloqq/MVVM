using System;
using System.Threading;
using System.Threading.Tasks;
#if PROJECT_SUPPORT_UNITASK
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
using Disposable;
using CompositeDisposable = Disposable.CompositeDisposable;

namespace Azzazelloqq.MVVM.Core
{
/// <summary>
/// Represents a base class for views in the MVVM architecture that are MonoBehaviour-based.
/// Implements <see cref="IView"/> and supports a view model of type <typeparamref name="TViewModel"/>.
/// Inherits from <see cref="MonoBehaviourDisposable"/> to provide both MonoBehaviour functionality and disposal management.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model associated with the view, which implements <see cref="IViewModel"/>.</typeparam>
public abstract class ViewMonoBehavior<TViewModel> : MonoBehaviourDisposable,  IView where TViewModel : IViewModel
{
#if PROJECT_SUPPORT_R3
    public ReadOnlyReactiveProperty<bool> IsInitialized => _isInitialized;
#else
    public IReadOnlyReactiveProperty<bool> IsInitialized => _isInitialized;
#endif
    
    /// <summary>
    /// The view model associated with the view.
    /// </summary>
    protected TViewModel viewModel;
    
    /// <summary>
    /// Gets the cancellation token that is triggered when the view is disposed.
    /// </summary>
    protected CancellationToken disposeToken => disposeCancellationToken;
	
    /// <summary>
    /// A composite disposable that manages the disposal of both the view and the model, along with other disposable resources.
    /// </summary>
    protected readonly ICompositeDisposable compositeDisposable = new CompositeDisposable();

    private bool _disposeWithViewModel;
#if PROJECT_SUPPORT_R3
    private readonly ReactiveProperty<bool> _isInitialized = new(false);
    private IDisposable _viewModelDisposeSubscription;
#else
    private readonly ReactiveProperty<bool> _isInitialized = new();
#endif

    /// <summary>
    /// Asynchronously initializes the view. This method must be called after the model is created 
    /// to set up any necessary state or dependencies. Failure to call this method may result in incorrect behavior.
    /// </summary>
    /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete. 
    /// It allows the operation to be canceled.</param>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    public async MVVMTask InitializeAsync(IViewModel viewModel, CancellationToken token, bool disposeWithViewModel = true)
    {
        if (_isInitialized.Value)
        {
            throw new Exception("View is already initialized");
        }
        
        if(viewModel is not TViewModel concreteViewModel)
        {
            throw new Exception($"Cannot cast {viewModel.GetType()} to {typeof(TViewModel)}");
        }

        this.viewModel = concreteViewModel;
        
        _disposeWithViewModel = disposeWithViewModel;
        SubscribeToDisposeNotifier(concreteViewModel);
        
        await OnInitializeAsync(token);
        
        MarkInitialized();
    }
    
    /// <summary>
    /// Initializes the view. This method must be called after the model is created to set up any necessary state or dependencies.
    /// Failure to call this method may result in incorrect behavior.
    /// </summary>
    public void Initialize(IViewModel viewModel, bool disposeWithViewModel = true)
    {
        if (_isInitialized.Value)
        {
            throw new Exception("View is already initialized");
        }
        
        if(viewModel is not TViewModel concreteViewModel)
        {
            throw new Exception($"Cannot cast {viewModel.GetType()} to {typeof(TViewModel)}");
        }

        this.viewModel = concreteViewModel;
        
        _disposeWithViewModel = disposeWithViewModel;
        SubscribeToDisposeNotifier(concreteViewModel);

        OnInitialize();
        
        MarkInitialized();
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
        
        // Unsubscribe from ViewModel's dispose notifier if it was initialized
        if (viewModel != null)
        {
            UnsubscribeFromDisposeNotifier();
        }
    }
    
    public sealed override async ValueTask DisposeAsync(CancellationToken token, bool continueOnCapturedContext = false)
    {
        await base.DisposeAsync(token, continueOnCapturedContext);
		
        await OnDisposeAsync(token);
		
        await compositeDisposable.DisposeAsync(token);
        
        // Unsubscribe from ViewModel's dispose notifier if it was initialized
        if (viewModel != null)
        {
            UnsubscribeFromDisposeNotifier();
        }
    }

    /// <summary>
    /// Adds a disposable resource to the composite disposable associated with this view.
    /// This ensures that the disposable resource will be properly disposed of when the view is disposed.
    /// </summary>
    /// <param name="disposable">The disposable resource to be added to the composite disposable.</param>
    protected void AddDisposable(IDisposable disposable)
    {
        compositeDisposable.AddDisposable(disposable);
    }

    /// <summary>
    /// Adds an asynchronous disposable object to the composite disposable collection.
    /// This ensures the object is properly disposed when the composite disposable is disposed.
    /// </summary>
    /// <param name="disposable">An object that implements <see cref="IAsyncDisposable"/> to be added
    /// to the composite disposable collection for managed disposal.</param>
    protected void AddAsyncDisposable(IAsyncDisposable disposable)
    {
        compositeDisposable.AddAsyncDisposable(disposable);
    }

    
    /// <summary>
    /// Provides a hook for subclasses to perform custom initialization logic.
    /// This method is called by the <see cref="Initialize"/> method.
    /// </summary>
    protected abstract void OnInitialize();

    /// <summary>
    /// Provides a hook for subclasses to perform custom asynchronous initialization logic.
    /// This method is called by the <see cref="InitializeAsync(CancellationToken)"/> method and can be overridden 
    /// in derived classes to implement specific asynchronous initialization behavior.
    /// </summary>
    /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete. 
    /// It allows the operation to be canceled.</param>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    protected abstract ValueTask OnInitializeAsync(CancellationToken token);
    
	protected abstract ValueTask OnDisposeAsync(CancellationToken token);
    
    /// <summary>
    /// Provides additional dispose logic for derived classes.
    /// Subclasses can override this method to implement custom cleanup code
    /// without overriding the base <see cref="Dispose"/> method.
    /// </summary>
    protected abstract void OnDispose();

    private void OnViewModelDisposed()
    {
        if (!_disposeWithViewModel)
        {
            return;
        }
        
        Dispose();
    }

    private void MarkInitialized()
    {
#if !PROJECT_SUPPORT_R3
        _isInitialized.SetValue(true);
#else
        _isInitialized.Value = true;
#endif
    }

    private void SubscribeToDisposeNotifier(TViewModel owner)
    {
#if !PROJECT_SUPPORT_R3
        owner.DisposeNotifier.Subscribe(OnViewModelDisposed);
#else
        _viewModelDisposeSubscription?.Dispose();
        _viewModelDisposeSubscription = owner.DisposeNotifier.Subscribe(_ => OnViewModelDisposed());
#endif
    }

    private void UnsubscribeFromDisposeNotifier()
    {
#if !PROJECT_SUPPORT_R3
        viewModel.DisposeNotifier.Unsubscribe(OnViewModelDisposed);
#else
        _viewModelDisposeSubscription?.Dispose();
        _viewModelDisposeSubscription = null;
#endif
    }
}
}

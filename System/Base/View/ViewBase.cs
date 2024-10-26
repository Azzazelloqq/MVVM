﻿using System.Threading;
using System.Threading.Tasks;
using Disposable;
using MVVM.MVVM.System.Base.ViewModel;

namespace MVVM.MVVM.System.Base.View
{
/// <summary>
/// Represents the base class for views in the MVVM architecture.
/// Implements <see cref="IView"/> and provides support for a view model of type <typeparamref name="TViewModel"/>.
/// Inherits from <see cref="DisposableBase"/> to provide disposal functionality.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model associated with the view, which implements <see cref="IViewModel"/>.</typeparam>
public abstract class ViewBase<TViewModel> : DisposableBase, IView where TViewModel : IViewModel
{
    /// <summary>
    /// The view model associated with the view.
    /// </summary>
    protected TViewModel viewModel;
    
    /// <summary>
    /// A composite disposable that manages the disposal of both the view and the model, along with other disposable resources.
    /// </summary>
    protected readonly ICompositeDisposable compositeDisposable = new CompositeDisposable();
    
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
    /// Initializes the view. This method must be called after the view is created to set up any necessary state or dependencies.
    /// Failure to call this method may result in incorrect behavior.
    /// </summary>
    public void Initialize(TViewModel viewModel)
    {
        this.viewModel = viewModel;
        
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
    public override void Dispose()
    {
        base.Dispose();
        
        OnDispose();
        
        compositeDisposable.Dispose();
    }

    /// <summary>
    /// Provides additional dispose logic for derived classes.
    /// Subclasses can override this method to implement custom cleanup code
    /// without overriding the base <see cref="Dispose"/> method.
    /// </summary>
    protected virtual void OnDispose()
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
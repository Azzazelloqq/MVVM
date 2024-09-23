using System.Threading;
using System.Threading.Tasks;
using Disposable;
using MVVM.MVVM.System.Base.Model;

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
    /// Initializes a new instance of the <see cref="ViewModelBase{TModel}"/> class with the specified model.
    /// </summary>
    /// <param name="model">The model to associate with the view model.</param>
    public ViewModelBase(TModel model)
    {
        this.model = model;
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
        await OnInitializeAsync(token);
    }
    
    /// <summary>
    /// Initializes the view with the specified view model.
    /// </summary>
    /// <param name="viewModel">The view model to associate with the view.</param>
    public void Initialize()
    {
        OnInitialize();
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
}
}
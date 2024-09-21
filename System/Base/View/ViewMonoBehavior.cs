using Disposable;
using MVVM.MVVM.System.Base.ViewModel;

namespace MVVM.MVVM.System.Base.View
{
/// <summary>
/// Represents a base class for views in the MVVM architecture that are MonoBehaviour-based.
/// Implements <see cref="IView"/> and supports a view model of type <typeparamref name="TViewModel"/>.
/// Inherits from <see cref="MonoBehaviourDisposable"/> to provide both MonoBehaviour functionality and disposal management.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model associated with the view, which implements <see cref="IViewModel"/>.</typeparam>
public abstract class ViewMonoBehavior<TViewModel> : MonoBehaviourDisposable, IView where TViewModel : IViewModel
{
    /// <summary>
    /// The view model associated with the view.
    /// </summary>
    protected TViewModel viewModel;
    
    /// <summary>
    /// Initializes the view with the specified view model.
    /// </summary>
    /// <param name="viewModel">The view model to associate with the view.</param>
    public void Initialize(TViewModel viewModel)
    {
        this.viewModel = viewModel;
        
        OnInitialize();
    }

    protected virtual void OnInitialize()
    {
        
    }
    
    /// <summary>
    /// Finalizer that ensures unmanaged resources are properly disposed of.
    /// Calls <see cref="Dispose"/> with <c>false</c> to avoid disposing managed resources.
    /// </summary>
    ~ViewMonoBehavior()
    {
        Dispose(false);
    }
}
}
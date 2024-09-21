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
}
}
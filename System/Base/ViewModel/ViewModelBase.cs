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

    public void Initialize()
    {
        OnInitialize();
    }

    protected virtual void OnInitialize()
    {
        
    }
}
}
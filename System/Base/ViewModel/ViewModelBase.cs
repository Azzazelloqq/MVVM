using Disposable;
using MVVM.MVVM.System.Base.Model;

namespace MVVM.MVVM.System.Base.ViewModel
{
public abstract class ViewModelBase<TModel> : DisposableBase,
    IViewModel where TModel : IModel
{
    protected TModel model;
    public ViewModelBase(TModel model)
    {
        this.model = model;
    }
}
}
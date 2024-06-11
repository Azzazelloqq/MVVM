using Disposable;
using MVVM.MVVM.System.Base.ViewModel;
using NotImplementedException = System.NotImplementedException;

namespace MVVM.MVVM.System.Base.View
{
public abstract class ViewBase<TViewModel> : DisposableBase, IView where TViewModel : IViewModel
{
    protected TViewModel viewModel;
    public virtual void Initialize(TViewModel viewModel)
    {
        this.viewModel = viewModel;
    }
}
}
using Disposable;
using MVVM.MVVM.System.Base.ViewModel;

namespace MVVM.MVVM.System.Base.View
{
public abstract class ViewMonoBehavior<TViewModel> : MonoBehaviourDisposable, IView where TViewModel : IViewModel
{
    protected TViewModel viewModel;
    public virtual void Initialize(TViewModel viewModel)
    {
        this.viewModel = viewModel;
    }
    
    ~ViewMonoBehavior()
    {
        Dispose(false);
    }
}
}
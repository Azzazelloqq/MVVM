using System;
using MVVM.MVVM.System.Base.ViewModel;
using UnityEngine;

namespace MVVM.MVVM.System.Base.View
{
public abstract class ViewMonoBehavior<TViewModel> : MonoBehaviour, IView, IDisposable where TViewModel : IViewModel
{
    protected bool disposed = false;
    protected bool isDestroyed = false;
    protected TViewModel viewModel;
    public virtual void Initialize(TViewModel viewModel)
    {
        this.viewModel = viewModel;
    }
    
    ~ViewMonoBehavior()
    {
        Dispose(false);
    }
    
    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
        {
            return;
        }

        if (disposing)
        {
            DisposeManagedResources();
        }

        DisposeUnmanagedResources();

        if (!isDestroyed)
        {
            Destroy(gameObject);
            isDestroyed = true;
        }
        
        disposed = true;
    }

    protected virtual void DisposeManagedResources()
    {
    }

    protected virtual void DisposeUnmanagedResources()
    {
    }
}
}
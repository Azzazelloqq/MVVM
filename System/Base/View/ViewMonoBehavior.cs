using System;
using UnityEngine;

namespace MVVM.MVVM.System.Base.View
{
public abstract class ViewMonoBehavior : MonoBehaviour, IView, IDisposable
{
    protected bool disposed = false;
    protected bool isDestroyed = false;

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
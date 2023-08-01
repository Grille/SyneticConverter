using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Graphics;
public abstract class GLStateObject : IDisposable
{
    bool _disposed;

    public void Bind()
    {
        OnBind();
    }

    public void Destroy()
    {
        if (_disposed)
            return;

        OnDestroy();

        _disposed = true;
    }

    protected abstract void OnBind();

    protected abstract void OnDestroy();

    ~GLStateObject() => Destroy();

    public void Dispose()
    {
        Destroy();
        GC.SuppressFinalize(this);
    }
}
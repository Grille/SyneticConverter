using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Graphics;
public abstract class GLObject : IDisposable
{
    public Context Context { get; }
    bool _disposed;

    public GLObject(Context ctx)
    {
        Context = ctx;
    }

    public void Bind()
    {
        OnBind();
    }

    protected abstract void OnBind();

    protected abstract void OnDelete();

    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        OnDelete();

        _disposed = true;
    }

    ~GLObject() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
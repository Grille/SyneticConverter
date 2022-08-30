using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Graphics;
public abstract class GLStateObject : IDisposable
{
    public GLState State { private set; get; } = GLState.None;

    public bool TryCreate()
    {
        if (State != GLState.Ready)
            Create();

        return State == GLState.Ready;
    }

    public void Create()
    {
        OnCreate();

        State = GLState.Ready;
    }

    public void Bind()
    {
        OnBind();
    }

    public void Destroy()
    {
        if (State != GLState.Ready)
            return;

        OnDestroy();

        State = GLState.None;
    }

    public void Update()
    {
        Destroy();
        Create();
    }

    protected abstract void OnCreate();

    protected abstract void OnBind();

    protected abstract void OnDestroy();

    ~GLStateObject() => Destroy();

    public void Dispose()
    {
        Destroy();
        GC.SuppressFinalize(this);
    }
}


public enum GLState
{
    None,
    Ready,
    Failed,
}

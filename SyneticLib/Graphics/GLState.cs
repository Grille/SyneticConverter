using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Graphics;
public interface IHasGLState : IDisposable
{
    public GLState GLState { get; }


}

public enum GLState
{
    None,
    Ready,
    Failed,
}

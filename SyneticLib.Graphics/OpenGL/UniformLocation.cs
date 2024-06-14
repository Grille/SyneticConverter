using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Graphics.OpenGL;

public struct UniformLocation
{
    public readonly int Location;
    public readonly bool Enabled;

    public UniformLocation(int location)
    {
        Location = location;
        Enabled = location != -1;
    }
}

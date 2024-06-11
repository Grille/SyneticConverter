using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace SyneticLib.Math3D;
public struct Ray
{
    public Vector3 Origin;
    public Vector3 Direction;

    public Ray(Vector3 origin, Vector3 direction)
    {
        Origin = origin;
        Direction = direction;
    }
}

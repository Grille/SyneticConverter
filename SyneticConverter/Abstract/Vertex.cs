using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SyneticConverter;
public struct Vertex
{
    public Vector3 Position;
    public Vector4 Normal;
    public Vector2 UV;
    public Vector3 Blending;
    public BgraColor Color;
    public float Shadow;
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SyneticLib;
public class MeshVertex
{
    public Vector3 Position;
    public Vector4 Normal;
    public Vector2 UV0;
    public Vector2 UV1;
    public Vector3 Blending;
    public BgraColor LightColor;
    public float Shadow;
}

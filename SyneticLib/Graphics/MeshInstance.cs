using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using OpenTK.Mathematics;

namespace SyneticLib.Graphics;
public class MeshInstance
{
    public Mesh Mesh;
    public Matrix4 ModelMatrix;

    public MeshInstance(Mesh mesh): this(mesh, Matrix4.Identity)
    {

    }

    public MeshInstance(Mesh mesh, Matrix4 modelMatrix)
    {
        Mesh = mesh;
        ModelMatrix = modelMatrix;
    }
}

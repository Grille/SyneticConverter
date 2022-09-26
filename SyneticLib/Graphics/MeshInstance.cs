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
    public Mesh Model; 
    public Matrix4 ModelMatrix;

    public MeshInstance(Mesh mesh): this(mesh, Matrix4.Identity)
    {

    }

    public MeshInstance(Mesh mesh, Matrix4 modelMatrix)
    {
        Model = mesh;
        ModelMatrix = modelMatrix;
    }

    public static implicit operator MeshInstance(Mesh mesh) => new MeshInstance(mesh);
}

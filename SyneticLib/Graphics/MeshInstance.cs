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
        if (mesh == null)
            throw new ArgumentNullException(nameof(mesh));

        Model = mesh;
        ModelMatrix = modelMatrix;
    }

    public static implicit operator MeshInstance(Mesh mesh) => new MeshInstance(mesh);

    public static explicit operator MeshInstance(PropInstance instance)
    {
        return new MeshInstance(instance.Class.Mesh, Matrix4.CreateTranslation(instance.Position.X, instance.Position.Y, instance.Position.Z));
    }
}

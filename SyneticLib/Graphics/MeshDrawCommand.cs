using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using OpenTK.Mathematics;
using static OpenTK.Graphics.OpenGL.GL;

namespace SyneticLib.Graphics;
public record class MeshDrawCommand
{
    public Mesh Mesh; 
    public Matrix4 ModelMatrix;
    public int RegionOffset;
    public int RegionCount;

    public MeshDrawCommand(Mesh mesh): this(mesh, Matrix4.Identity) { }

    public MeshDrawCommand(Mesh mesh, Matrix4 modelMatrix) : this(mesh, modelMatrix, 0, mesh.MaterialRegion.Length) { }

    public MeshDrawCommand(Mesh mesh, Matrix4 modelMatrix, int regionOffset, int regionCount) 
    {
        if (mesh == null)
            throw new ArgumentNullException(nameof(mesh));

        Mesh = mesh;
        ModelMatrix = modelMatrix;
        RegionOffset = regionOffset;
        RegionCount = regionCount;
    }

    public static implicit operator MeshDrawCommand(MeshSectionPtr ptr) => new MeshDrawCommand(ptr.Target, Matrix4.Identity, ptr.Offset, ptr.Count);

    public static implicit operator MeshDrawCommand(Mesh mesh) => new MeshDrawCommand(mesh);

    public static explicit operator MeshDrawCommand(PropInstance instance)
    {
        return new MeshDrawCommand(instance.Class.Mesh, Matrix4.CreateTranslation(instance.Position.X, instance.Position.Y, instance.Position.Z));
    }
}

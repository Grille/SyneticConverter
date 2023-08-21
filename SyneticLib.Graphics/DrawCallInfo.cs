using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using OpenTK.Mathematics;
using static OpenTK.Graphics.OpenGL.GL;
using OpenTK.Compute.OpenCL;

namespace SyneticLib.Graphics;
public class DrawCallInfo
{
    public MeshBufferRegionInfo Mesh { get; }
    public MaterialProgram Material { get; }
    public Matrix4 Matrix { get; }
    public TextureBindingInfo[] Textures { get; }

    public DrawCallInfo(MeshBufferRegionInfo mesh, MaterialProgram material, Matrix4 matrix)
    {
        Mesh = mesh;
        Material = material;
        Matrix = matrix;
        //Textures = Material.TextureBindings;
    }

    public void BindTextures()
    {
        for (int i = 0; i < Textures.Length; i++)
        {
            Textures[i].Bind();
        }
    }

    public void DrawElements()
    {
        Mesh.DrawElements();
    }
}

public record class TextureBindingInfo(int Sampler, TextureBuffer Texture)
{
    public void Bind()
    {
        Texture.Bind(Sampler);
    }
}

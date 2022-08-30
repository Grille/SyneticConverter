using System;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace SyneticLib.Graphics;
public class MeshMaterialProgram : GLProgram
{
    public int UColorAmbient {get; protected set;}
    public int UColorDiffuse { get; protected set; }
    public int UColorSpec1 { get; protected set; }
    public int UColorSpec2 { get; protected set; }
    public int UColorReflect { get; protected set; }


    public readonly ModelMaterial Owner;
    public MeshMaterialProgram(ModelMaterial material)
    {
        Owner = material;
    }

    protected override void OnCreate()
    {
        Compile(GLSLSource.MeshVertex, GLSLSource.MeshFragment);

        UModelMatrix4 = GetUniformLocation("uModel");
        UViewMatrix4 = GetUniformLocation("uView");
        UProjectionMatrix4 = GetUniformLocation("uProjection");

        UColorAmbient = GetUniformLocation("uColorAmbient");
        UColorDiffuse = GetUniformLocation("uColorDiffuse");

        GL.UseProgram(ProgramID);
        GL.Uniform3(UColorDiffuse, Owner.Diffuse.R / 255f, Owner.Diffuse.G / 255f, Owner.Diffuse.B / 255f);
    }
}

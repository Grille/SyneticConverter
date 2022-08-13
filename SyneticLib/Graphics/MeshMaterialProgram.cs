using System;
using OpenTK.Graphics.OpenGL4;

namespace SyneticLib.Graphics;
public class MeshMaterialProgram : GLStateObject
{
    internal MeshMaterial owner;
    internal int VertexID;
    internal int FragmentID;
    internal int ProgramID;


    public MeshMaterialProgram(MeshMaterial material)
    {
        owner = material;
    }

    static MeshMaterialProgram()
    {
        Default = new MeshMaterialProgram(null);
    }

    public static MeshMaterialProgram Default;

    protected override void OnCreate()
    {
        VertexID = GL.CreateShader(ShaderType.VertexShader);
        FragmentID = GL.CreateShader(ShaderType.FragmentShader);

        GL.ShaderSource(VertexID, GLSLSrc.BasicVertexSrc);
        GL.CompileShader(VertexID);
        GL.GetShaderInfoLog(VertexID, out string vertlog);

        GL.ShaderSource(FragmentID, GLSLSrc.BasicFragmentSrc);
        GL.CompileShader(FragmentID);
        GL.GetShaderInfoLog(FragmentID, out string idxlog);

        ProgramID = GL.CreateProgram();
        GL.AttachShader(ProgramID, VertexID);
        GL.AttachShader(ProgramID, FragmentID);
        GL.LinkProgram(ProgramID);

        GL.DeleteShader(VertexID);
        GL.DeleteShader(FragmentID);
    }

    protected override void OnBind()
    {
        GL.UseProgram(ProgramID);
    }

    protected override void OnDestroy()
    {
        GL.DeleteProgram(ProgramID);
    }

}

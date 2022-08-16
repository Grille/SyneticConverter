using System;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace SyneticLib.Graphics;
public class MeshMaterialProgram : GLStateObject
{
    internal MeshMaterial owner;
    internal int VertexID;
    internal int FragmentID;
    internal int ProgramID;

    internal int UModelMatrix4;
    internal int UViewMatrix4;
    internal int UProjectionMatrix4;


    public MeshMaterialProgram(MeshMaterial material)
    {
        owner = material;
    }

    protected override void OnCreate()
    {
        VertexID = GL.CreateShader(ShaderType.VertexShader);
        FragmentID = GL.CreateShader(ShaderType.FragmentShader);

        GL.ShaderSource(VertexID, GLSLSrc.MeshVertexSrc);
        GL.CompileShader(VertexID);
        GL.GetShaderInfoLog(VertexID, out string vertlog);

        GL.ShaderSource(FragmentID, GLSLSrc.FragmentSrc);
        GL.CompileShader(FragmentID);
        GL.GetShaderInfoLog(FragmentID, out string idxlog);

        ProgramID = GL.CreateProgram();
        GL.AttachShader(ProgramID, VertexID);
        GL.AttachShader(ProgramID, FragmentID);
        GL.LinkProgram(ProgramID);

        GL.DeleteShader(VertexID);
        GL.DeleteShader(FragmentID);

        UModelMatrix4 = GL.GetUniformLocation(ProgramID, "uModel");
        UViewMatrix4 = GL.GetUniformLocation(ProgramID, "uView");
        UProjectionMatrix4 = GL.GetUniformLocation(ProgramID, "uProjection");
    }

    public void SubCameraMatrix(Camera camera)
    {
        GL.UniformMatrix4(UViewMatrix4, false, ref camera.ViewMatrix);
        GL.UniformMatrix4(UProjectionMatrix4, false, ref camera.ProjectionMatrix);
    }

    public void SubModelMatrix(Matrix4 matrix)
    {
        GL.UniformMatrix4(UModelMatrix4, false, ref matrix);
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

using System;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace SyneticLib.Graphics;
public abstract class GLProgram : GLStateObject
{
    internal int VertexID;
    internal int FragmentID;
    internal int ProgramID;

    internal int UModelMatrix4;
    internal int UViewMatrix4;
    internal int UProjectionMatrix4;

    protected void Compile(string vertex, string fragment)
    {
        VertexID = GL.CreateShader(ShaderType.VertexShader);
        FragmentID = GL.CreateShader(ShaderType.FragmentShader);

        GL.ShaderSource(VertexID, vertex);
        GL.CompileShader(VertexID);
        GL.GetShaderInfoLog(VertexID, out string vertlog);
        if (vertlog != "")
            throw new ArgumentException(vertlog);

        GL.ShaderSource(FragmentID, fragment);
        GL.CompileShader(FragmentID);
        GL.GetShaderInfoLog(FragmentID, out string idxlog);
        if (idxlog != "")
            throw new ArgumentException(idxlog);

        ProgramID = GL.CreateProgram();
        GL.AttachShader(ProgramID, VertexID);
        GL.AttachShader(ProgramID, FragmentID);
        GL.LinkProgram(ProgramID);

        GL.DeleteShader(VertexID);
        GL.DeleteShader(FragmentID);
    }

    protected int GetUniformLocation(string name) => GL.GetUniformLocation(ProgramID, name);

    public void SubMatrix4(int location, ref Matrix4 matrix)
    {
        GL.UniformMatrix4(location, false, ref matrix);
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

    protected sealed override void OnBind()
    {
        GL.UseProgram(ProgramID);
    }

    protected sealed override void OnDestroy()
    {
        GL.DeleteProgram(ProgramID);
    }

}

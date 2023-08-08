using System;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.CompilerServices;

namespace SyneticLib.Graphics;
public abstract class GLProgram : GLObject
{
    readonly int ProgramID;

    internal int UModelMatrix4;
    internal int UViewMatrix4;
    internal int UProjectionMatrix4;

    static GLProgram bound;

    public GLProgram()
    {
        ProgramID = GL.CreateProgram();
    }

    protected void Compile(string vertex, string fragment)
    {
        var vertexID = GL.CreateShader(ShaderType.VertexShader);
        var fragmentID = GL.CreateShader(ShaderType.FragmentShader);

        GL.ShaderSource(vertexID, vertex);
        GL.CompileShader(vertexID);
        GL.GetShaderInfoLog(vertexID, out string vertlog);
        if (vertlog != "")
            throw new ArgumentException(vertlog);

        GL.ShaderSource(fragmentID, fragment);
        GL.CompileShader(fragmentID);
        GL.GetShaderInfoLog(fragmentID, out string idxlog);
        if (idxlog != "")
            throw new ArgumentException(idxlog);


        GL.AttachShader(ProgramID, vertexID);
        GL.AttachShader(ProgramID, fragmentID);
        GL.LinkProgram(ProgramID);

        GL.DeleteShader(vertexID);
        GL.DeleteShader(fragmentID);
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

    public void SubModelMatrix(in Matrix4 matrix)
    {
        GL.UniformMatrix4(UModelMatrix4, false, ref Unsafe.AsRef(matrix));
    }

    protected sealed override void OnBind()
    {
        if (bound == this)
            return;

        GL.UseProgram(ProgramID);
        bound = this;
    }

    protected sealed override void OnDestroy()
    {
        GL.DeleteProgram(ProgramID);
    }

}

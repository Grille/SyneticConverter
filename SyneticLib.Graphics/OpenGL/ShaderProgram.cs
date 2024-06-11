using System;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.CompilerServices;

using SyneticLib.Math3D;
using SyneticLib.Graphics.Shaders;

namespace SyneticLib.Graphics.OpenGL;
public abstract class ShaderProgram : GLObject
{
    public int ProgramID { get; }

    protected int UModelMatrix4;
    protected int UViewMatrix4;
    protected int UProjectionMatrix4;

    public ShaderProgram()
    {
        ProgramID = GL.CreateProgram();
    }

    public void Link(GlslVertexShader vertex, GlslFragmentShader fragment)
    {
        using var shader = new Shader(vertex, fragment);
        Link(shader);
    }

    public void Link(MaterialShaderType shaderType)
    {
        using var shader = new Shader(shaderType);
        Link(shader);
    }

    public void Link(Shader shader)
    {
        GL.AttachShader(ProgramID, shader.VertexID);
        GL.AttachShader(ProgramID, shader.FragmentID);
        GL.LinkProgram(ProgramID);
    }

    protected int GetUniformLocation(string name) => GL.GetUniformLocation(ProgramID, name);

    public void SubMatrix4(int location, in Matrix4 matrix)
    {
        GL.UniformMatrix4(location, false, ref Unsafe.AsRef(matrix));
    }

    public void SubSingle(int location, float vec)
    {
        GL.Uniform1(location, vec);
    }

    public void SubVector3(int location, Vector3 vec)
    {
        GL.Uniform3(location, ref vec);
    }

    public void SubCameraMatrix(Camera camera)
    {
        SubMatrix4(UViewMatrix4, camera.ViewMatrix);
        SubMatrix4(UProjectionMatrix4, camera.ProjectionMatrix);
    }

    public void SubModelMatrix(in Matrix4 matrix)
    {
        SubMatrix4(UModelMatrix4, matrix);
    }

    protected sealed override void OnBind()
    {
        GL.UseProgram(ProgramID);
    }

    protected sealed override void OnDelete()
    {
        GL.DeleteProgram(ProgramID);
    }

}

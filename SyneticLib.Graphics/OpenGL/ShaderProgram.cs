using System;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.CompilerServices;

using SyneticLib.Math3D;
using SyneticLib.Graphics.Shaders;
using System.Collections.Generic;

namespace SyneticLib.Graphics.OpenGL;
public class ShaderProgram : GLObject
{
    public int ProgramID { get; }

    public ShaderProgram()
    {
        ProgramID = GL.CreateProgram();
    }

    public void Link(GlslVertexShaderSource vertex, GlslFragmentShaderSource fragment)
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

    protected UniformLocation GetUniformLocation(string name, bool throwIfNotFound = false)
    {
        int uniform = GL.GetUniformLocation(ProgramID, name);
        if (throwIfNotFound && uniform == -1)
        {
            throw new KeyNotFoundException($"Uniform {name} not found.");
        }
        return new UniformLocation(uniform);
    }

    public void SubMatrix4(UniformLocation location, in Matrix4 matrix)
    {
        GL.UniformMatrix4(location.Location, false, ref Unsafe.AsRef(matrix));
    }

    public void SubSingle(UniformLocation location, float vec)
    {
        GL.Uniform1(location.Location, vec);
    }

    public void SubVector2(UniformLocation location, Vector2 vec)
    {
        GL.Uniform2(location.Location, vec);
    }

    public void SubVector3(UniformLocation location, Vector3 vec)
    {
        GL.Uniform3(location.Location, vec);
    }

    protected sealed override void OnBind()
    {
        GLContext.Bind(this);
    }

    protected sealed override void OnDelete()
    {
        GL.DeleteProgram(ProgramID);
    }

}

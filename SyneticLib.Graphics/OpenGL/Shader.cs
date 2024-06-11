﻿using System;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.CompilerServices;
using SyneticLib.LowLevel;
using SyneticLib.Graphics.Shaders;
using System.Numerics;

namespace SyneticLib.Graphics.OpenGL;

public class Shader : GLObject
{
    public int VertexID { get; }
    public int FragmentID { get; }

    private Shader()
    {
        VertexID = GL.CreateShader(ShaderType.VertexShader);
        FragmentID = GL.CreateShader(ShaderType.FragmentShader);
    }

    public Shader(MaterialShaderType type) : this()
    {
        var files = GLSLSources.LoadInternalShaderFiles(type);
        Compile(files.Vert, files.Frag);
    }

    public Shader(GlslVertexShader vertex, GlslFragmentShader fragment) : this() 
    {
        Compile(vertex, fragment);
    }

    private void Compile(GlslVertexShader vertex, GlslFragmentShader fragment)
    {
        Compile(VertexID, vertex);
        Compile(FragmentID, fragment);
    }

    void Compile(int shader, string source)
    {
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);
        GL.GetShaderInfoLog(shader, out string log);
        if (!string.IsNullOrEmpty(log))
            throw new ArgumentException(log);
    }

    protected override void OnBind()
    {
        throw new NotSupportedException();
    }

    protected override void OnDelete()
    {
        GL.DeleteShader(VertexID);
        GL.DeleteShader(FragmentID);
    }
}

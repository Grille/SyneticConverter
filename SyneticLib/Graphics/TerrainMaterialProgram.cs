﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL4;

namespace SyneticLib.Graphics;
public class TerrainMaterialProgram : GLStateObject
{
    TerrainMaterial owner;
    internal int VertexID;
    internal int FragmentID;
    internal int ProgramID;

    internal int UViewMatrix4;
    internal int UProjectionMatrix4;

    public TerrainMaterialProgram(TerrainMaterial material)
    {
        owner = material;
    }

    protected override void OnBind()
    {
        GL.UseProgram(ProgramID);
    }

    protected override void OnCreate()
    {
        VertexID = GL.CreateShader(ShaderType.VertexShader);
        FragmentID = GL.CreateShader(ShaderType.FragmentShader);

        GL.ShaderSource(VertexID, GLSLSrc.TerrainVertexSrc);
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

        UViewMatrix4 = GL.GetUniformLocation(ProgramID, "uView");
        UProjectionMatrix4 = GL.GetUniformLocation(ProgramID, "uProjection");
    }

    public void SubCameraMatrix(Camera camera)
    {
        GL.UniformMatrix4(UViewMatrix4, false, ref camera.ViewMatrix);
        GL.UniformMatrix4(UProjectionMatrix4, false, ref camera.ProjectionMatrix);
    }

    protected override void OnDestroy()
    {
        GL.DeleteProgram(ProgramID);
    }
}

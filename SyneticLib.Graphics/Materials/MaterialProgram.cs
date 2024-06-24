using System;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using SyneticLib.Graphics.Shaders;
using SyneticLib.LowLevel;
using SyneticLib.Graphics.OpenGL;
using SyneticLib.Graphics.DrawCalls;
using System.Collections.Generic;
using SyneticLib.Math3D;

namespace SyneticLib.Graphics.Materials;
public class MaterialProgram : ShaderProgram
{
    public UniformLocation UModelMatrix4 { get; }
    public UniformLocation UViewMatrix4 { get; }
    public UniformLocation UProjectionMatrix4 { get; }

    public UniformLocation[] UTexture { get; }

    public MaterialProgram(Shader shader, int textureCount)
    {
        UTexture = new UniformLocation[textureCount];

        Link(shader);

        for (int i = 0; i < textureCount; i++)
        {
            UTexture[i] = GetUniformLocation("uTexture" + i, false);
        }

        UModelMatrix4 = GetUniformLocation("uModel");
        UViewMatrix4 = GetUniformLocation("uView");
        UProjectionMatrix4 = GetUniformLocation("uProjection");

        Bind();

        for (int i = 0; i < textureCount; i++)
        {
            var uniform = UTexture[i];

            if (!uniform.Enabled)
                continue;

            GL.Uniform1(uniform.Location, i);
        }
    }

    public virtual void ApplyUniforms(MaterialUniforms uniforms)
    {

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
}

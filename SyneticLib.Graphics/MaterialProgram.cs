using System;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using SyneticLib.Graphics.Shaders;
using SyneticLib.LowLevel;

namespace SyneticLib.Graphics;
public class MaterialProgram : GLProgram
{
    public int UColorAmbient {get; protected set;}
    public int UColorDiffuse { get; protected set; }
    public int UColorSpec1 { get; protected set; }
    public int UColorSpec2 { get; protected set; }
    public int UColorReflect { get; protected set; }

    public TextureBindingInfo TextureBinding0 { get; }
    //public TextureBindingInfo TextureBinding0 { get; }
    //public TextureBindingInfo TextureBinding0 { get; }

    public MaterialProgram(Context ctx, Material material) : base(ctx)
    {
        using (var shader = new Shader(ctx, material.ShaderType))
        {
            GL.AttachShader(ProgramID, shader.VertexID);
            GL.AttachShader(ProgramID, shader.FragmentID);
            GL.LinkProgram(ProgramID);
        }

        TextureBinding0 = new TextureBindingInfo(0, ctx.Create(material.TexSlot0.Texture));

        UModelMatrix4 = GetUniformLocation("uModel");
        UViewMatrix4 = GetUniformLocation("uView");
        UProjectionMatrix4 = GetUniformLocation("uProjection");

        UColorAmbient = GetUniformLocation("uColorAmbient");
        UColorDiffuse = GetUniformLocation("uColorDiffuse");

        Bind();
        GL.Uniform3(UColorDiffuse, material.Diffuse);
    }


}

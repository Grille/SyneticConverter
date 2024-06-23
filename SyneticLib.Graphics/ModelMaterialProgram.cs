using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Graphics.Shaders;

using SyneticLib.Graphics.OpenGL;

namespace SyneticLib.Graphics;

public class ModelMaterialProgram : MaterialProgram
{
    public UniformLocation UColorAmbient { get; protected set; }
    public UniformLocation UColorDiffuse { get; protected set; }
    public UniformLocation UColorSpec1 { get; protected set; }
    public UniformLocation UColorSpec2 { get; protected set; }
    public UniformLocation UColorReflect { get; protected set; }

    public ModelMaterialProgram(ModelMaterial material, GlObjectCache<Texture, TextureBuffer>? textures = null) : base(material, textures, GetShader(material))
    {
        UColorAmbient = GetUniformLocation("uColorAmbient", false);
        UColorDiffuse = GetUniformLocation("uColorDiffuse", false);

        SubVector3(UColorDiffuse, material.Diffuse);
    }

    static Shader GetShader(ModelMaterial material)
    {
        return new Shader(GLSLSources.VModel, GLSLSources.Model);
    }
}

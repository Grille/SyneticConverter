using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Graphics.Shaders;

using SyneticLib.Graphics.OpenGL;

namespace SyneticLib.Graphics.Materials;

public class ModelMaterialProgram : MaterialProgram
{
    public UniformLocation UColorAmbient { get; protected set; }
    public UniformLocation UColorDiffuse { get; protected set; }
    public UniformLocation UColorSpec1 { get; protected set; }
    public UniformLocation UColorSpec2 { get; protected set; }
    public UniformLocation UColorReflect { get; protected set; }

    public ModelMaterialProgram(Shader shader) : base(shader, 10)
    {
        UColorAmbient = GetUniformLocation("uColorAmbient", false);
        UColorDiffuse = GetUniformLocation("uColorDiffuse", false);
    }

    public void ApplyUniforms(ModelMaterialUniforms uniforms)
    {
        SubVector3(UColorDiffuse, uniforms.Diffuse);
    }

    static Shader GetShader(ModelMaterial material)
    {
        return new Shader(GLSLSources.VModel, GLSLSources.Model);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

using SyneticLib.Graphics.OpenGL;
using SyneticLib.Graphics.Shaders;

namespace SyneticLib.Graphics;

public class TerrainMaterialProgram : MaterialProgram
{
    UniformLocation UVMatrix0;
    UniformLocation UVMatrix1;
    UniformLocation UVMatrix2;

    UniformLocation Mode0;
    UniformLocation Mode1;

    public TerrainMaterialProgram(TerrainMaterial material, GlObjectCache<Texture, TextureBuffer>? textures = null) : base(material, textures, GetShader(material))
    {
        UVMatrix0 = GetUniformLocation("UVMatrix0");
        UVMatrix1 = GetUniformLocation("UVMatrix1");
        UVMatrix2 = GetUniformLocation("UVMatrix2");

        Mode0 = GetUniformLocation("Mode0");
        Mode1 = GetUniformLocation("Mode1");
    }

    static Shader GetShader(TerrainMaterial material)
    {
        return new Shader(GLSLSources.VTerrain, GLSLSources.Terrain);
    }
}

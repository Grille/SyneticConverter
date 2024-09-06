using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;
using SyneticLib.Graphics.OpenGL;
using SyneticLib.Graphics.Shaders;
using SyneticLib.World;

namespace SyneticLib.Graphics.Materials;

public class TerrainMaterialProgram : MaterialProgram
{
    UniformLocation UVMatrix0;
    UniformLocation UVMatrix1;
    UniformLocation UVMatrix2;

    UniformLocation Mode0;
    UniformLocation Mode1;

    public TerrainMaterialProgram(Shader shader) : base(shader, 7)
    {
        UVMatrix0 = GetUniformLocation("uMat0");
        UVMatrix1 = GetUniformLocation("uMat1");
        UVMatrix2 = GetUniformLocation("uMat2");

        Mode0 = GetUniformLocation("uMode0");
        Mode1 = GetUniformLocation("uMode1");
    }

    public void ApplyUniforms(TerrainMaterialUniforms uniforms)
    {
        SubMatrix2x4(UVMatrix0, uniforms.UVMatrix0);
        SubMatrix2x4(UVMatrix1, uniforms.UVMatrix1);
        SubMatrix2x4(UVMatrix2, uniforms.UVMatrix2);

        SubInt32(Mode0, uniforms.Mode0);
        SubInt32(Mode1, uniforms.Mode1);
    }

    static Shader GetShader(TerrainMaterial material)
    {
        return new Shader(GLSLSources.VTerrain, GLSLSources.TerrainWR2);
    }
}

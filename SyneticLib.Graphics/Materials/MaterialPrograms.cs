using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Graphics.Shaders;
using SyneticLib.Graphics.OpenGL;

namespace SyneticLib.Graphics.Materials;

public class MaterialPrograms : IDisposable
{
    public ModelMaterialProgram DefaultModel { get; }

    public TerrainMaterialProgram DefaultTerrain { get; }

    public FrameShaderProgram FrameShader { get; }

    public MaterialPrograms()
    {

        using (var shader = new Shader(GLSLSources.VModel, GLSLSources.Model))
        {
            DefaultModel = new ModelMaterialProgram(shader);
        }

        using (var shader = new Shader(GLSLSources.VTerrain, GLSLSources.Terrain))
        {
            DefaultTerrain = new TerrainMaterialProgram(shader);
        }

        FrameShader = new FrameShaderProgram(GLSLSources.Frame);
    }

    public void Dispose()
    {
        DefaultModel.Dispose();
        DefaultTerrain.Dispose();
        FrameShader.Dispose();
    }
}

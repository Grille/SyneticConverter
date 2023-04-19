using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL4;
using SyneticLib.Graphics.Shaders;

namespace SyneticLib.Graphics;
public class TerrainMaterialProgram : GLProgram
{
    public TerrainMaterial Owner;
    public TerrainMaterialProgram(TerrainMaterial material)
    {
        Owner = material;
    }

    protected override void OnCreate()
    {
        Compile(GLSLSources.TerrainVertex, GLSLSources.TerrainFragment);

        UViewMatrix4 = GetUniformLocation("uView");
        UProjectionMatrix4 = GetUniformLocation("uProjection");
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL4;

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
        Compile(GLSLSource.TerrainVertex, GLSLSource.TerrainFragment);

        UViewMatrix4 = GetUniformLocation("uView");
        UProjectionMatrix4 = GetUniformLocation("uProjection");
    }
}

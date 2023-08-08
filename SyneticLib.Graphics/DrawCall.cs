using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using OpenTK.Mathematics;
using static OpenTK.Graphics.OpenGL.GL;
using OpenTK.Compute.OpenCL;

namespace SyneticLib.Graphics;
public record class DrawCall(GLMeshBuffer Mesh, GLProgram Program, Matrix4 Matrix, int ElementOffset, int ElementCount)
{
    public void Execute()
    {

        
    }
}

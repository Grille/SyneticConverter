using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace SyneticLib.Graphics.OpenGL;

public static class GLContext
{
    static ShaderProgram? _usedProgram;

    public static void Bind(ShaderProgram program)
    {
        if (program == _usedProgram)
        {
            return;
        }
        _usedProgram = program;
        GL.UseProgram(program.ProgramID);
    }
}

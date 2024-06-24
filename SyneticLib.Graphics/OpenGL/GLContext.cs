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
    public static ShaderProgram? UsedProgram { get; private set; }

    public static void Bind(ShaderProgram program)
    {
        if (program == UsedProgram)
        {
            return;
        }
        UsedProgram = program;
        GL.UseProgram(program.ProgramID);
    }

    public static void PrintError()
    {
        var error = GL.GetError();
        if (error != ErrorCode.NoError)
        {
            Console.WriteLine($"{DateTime.Now} {error}");
        }
    }

    public static void PrintError(string msg)
    {
        var error = GL.GetError();
        if (error != ErrorCode.NoError)
        {
            Console.WriteLine($"{DateTime.Now} {msg} {error}");
        }
    }
}

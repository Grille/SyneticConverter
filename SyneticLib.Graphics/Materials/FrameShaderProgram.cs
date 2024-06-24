using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Graphics.OpenGL;
using SyneticLib.Graphics.Shaders;

namespace SyneticLib.Graphics.Materials;

public class FrameShaderProgram : ShaderProgram
{
    UniformLocation Diffuse;
    UniformLocation Normal;
    UniformLocation Light;

    public FrameShaderProgram() : this(GLSLSources.Frame) { }

    public FrameShaderProgram(GlslFragmentShaderSource frag)
    {
        Link(GLSLSources.VFrame, frag);

        Diffuse = GetUniformLocation("Diffuse", false);
        Normal = GetUniformLocation("Normal", false);
        Light = GetUniformLocation("Light", false);

        Bind();

        SubInt32(Diffuse, 0);
        SubInt32(Normal, 1);
        SubInt32(Light, 2);
    }
}

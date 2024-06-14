using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Graphics.Shaders;

using SyneticLib.Graphics.OpenGL;

namespace SyneticLib.Graphics;

public class SpriteProgram : ShaderProgram
{
    public UniformLocation UOffset { get; protected set; }
    public UniformLocation UScale { get; protected set; }

    public SpriteProgram()
    {
        Link(GLSLSources.VSprite, GLSLSources.Sprite);

        UOffset = GetUniformLocation("uOffset");
        UScale = GetUniformLocation("uScale");
    }

    public void Sub(Sprite sprite)
    {
        SubVector2(UOffset, sprite.Offset);
        SubVector2(UScale, sprite.Scale);
    }
}

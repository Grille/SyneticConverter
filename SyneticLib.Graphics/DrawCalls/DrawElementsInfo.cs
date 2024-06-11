using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;


namespace SyneticLib.Graphics.DrawCalls;

public struct DrawElementsInfo
{
    public readonly nint Start;
    public readonly int Length;
    public readonly int Offset;

    public DrawElementsInfo(int start, int length, int offset = 0)
    {
        Start = start* sizeof(int);
        Length = length;
        Offset = offset;
    }

    public void Excecute()
    {
        GL.DrawElementsBaseVertex(PrimitiveType.Triangles, Length, DrawElementsType.UnsignedInt, Start, Offset);
    }
}

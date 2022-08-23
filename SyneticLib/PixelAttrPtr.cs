using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

public record class PixelAttrPtr
{
    public Channel R, G, B, A;
    public int Stride;

    public PixelAttrPtr(int r, int g, int b)
    {
        R.Enable(r);
        G.Enable(g);
        B.Enable(b);
        Stride = 3;
    }

    public PixelAttrPtr(int r, int g, int b, int a)
    {
        R.Enable(r);
        G.Enable(g);
        B.Enable(b);
        A.Enable(a);
        Stride = 4;
    }

    public static PixelAttrPtr RGB8 = new PixelAttrPtr(0, 1, 2);
    public static PixelAttrPtr RGBA8 = new PixelAttrPtr(0, 1, 2, 3);


    public struct Channel
    {
        public bool Enabled;
        public int Offset;
        public int Size;

        public void Enable(int offset)
        {
            Enabled = true;
            Offset = offset;
            Size = 1;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GGL.IO;

namespace SyneticConverter;
public class GeoFile : SyneticFile
{
    public MHead Head;

    public int[] VtxQty;
    public MVertex[] Vertices;


    public override void Read(BinaryViewReader br)
    {
        Head = br.Read<MHead>();

        VtxQty = br.ReadArray<int>(64);

        int length = 0;
        for (int i = 0; i < VtxQty.Length; i++)
            length += VtxQty[i];

        Vertices = br.ReadArray<MVertex>();
    }

    public override void Write(BinaryViewWriter bw)
    {
        throw new NotImplementedException();
    }

    public struct MHead
    {
        public String4 Magic;
        public int X2, X3, SizeX, SizeZ, XZ, Qty, Density;
    }

    public struct MVertex
    {
        public Vector3 Position;
        public BgraColor Normal;
        public Vector2 UV0;
        public Vector2 UV1;
        public BlendColor Blend;
        public BgraColor Color;
    }
}

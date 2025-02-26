using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grille.IO;

using OpenTK.Mathematics;

using SyneticLib.Files.Common;

namespace SyneticLib.Files;
public class TreeFile : BinaryFile, IVertexData, IIndexData
{
    public readonly static String4 Magic = (String4)"EERT";

    public MHead Head;

    public Vertex[] Vertecis { get; set; }
    public IdxTriangleInt32[] Indices { get; set; }

    public TreeFile()
    {
        Head.Magic = Magic;
        Vertecis = Array.Empty<Vertex>();
        Indices = Array.Empty<IdxTriangleInt32>();
    }

    public override void Deserialize(BinaryViewReader br)
    {
        Head = br.Read<MHead>();

        if (Head.Magic != Magic)
        {
            throw new InvalidDataException($"Invalid Head '{Head.Magic}'.");
        }
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        throw new NotImplementedException();
    }

    public struct MHead
    {
        public String4 Magic;
        public int VtxCount;
        public int SpriteCount;
        public int IdxCount;
        public String64 TextureBark;
        public String64 TextureLeaves;
    }

    public struct MHeadLodLevel
    {

    }

    public struct MVertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 UV0;

        public static explicit operator Vertex(MVertex src) => new Vertex()
        {
            Position = src.Position,
            Normal = src.Normal,
            UV0 = src.UV0,
        };

        public static explicit operator MVertex(Vertex src) => new MVertex()
        {
            Position = src.Position,
            Normal = src.Normal,
            UV0 = src.UV0,
        };
    }


    public struct MSprite
    {
        public Vector3 Position;
        public int Color;
    }
}

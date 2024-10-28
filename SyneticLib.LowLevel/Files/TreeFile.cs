using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grille.IO;
using SyneticLib.Files.Common;

namespace SyneticLib.Files;
public class TreeFile : BinaryFile
{
    public readonly static String4 Magic = (String4)"EERT";

    public MHead Head;

    public TreeFile()
    {
        Head.Magic = Magic;
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

    }

    public struct MSprite
    {

    }
}

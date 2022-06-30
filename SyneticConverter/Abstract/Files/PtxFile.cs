using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGL.IO;

namespace SyneticConverter;
internal class PtxFile : SyneticFile
{
    public MHead Head;
    public override void Read(BinaryViewReader br)
    {
        Head = br.Read<MHead>();

        for (int i = 0; i< Head.MipMaps; i++)
        {
            br.Read<MMipHead>();
        }
    }

    public override void Write(BinaryViewWriter bw)
    {
        throw new NotImplementedException();
    }

    public struct MHead
    {
        public byte Compression;
        public byte BitPerPixel;
        public byte Clear1;
        public byte Clear2;
        public int Width;
        public int Height;
        public byte MipMaps;
        public byte R, G, B;
    }

    public struct MMipHead
    {
        public int Size, SynSize;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grille.IO;
using SyneticLib.Files.Common;

namespace SyneticLib.Files;
public class SynFile : BinaryFile
{
    public MHead Head;
    public MFileEntry[] Entries;
    public byte[][] Data;

    public SynFile()
    {
        Entries = Array.Empty<MFileEntry>();
        Data = Array.Empty<byte[]>();
    }

    public override void Deserialize(BinaryViewReader br)
    {
        Head = br.Read<MHead>();
        br.Position += 52;

        Entries = br.ReadArray<MFileEntry>(Head.FileCount);

        Data = new byte[Head.FileCount][];

        for (int i = 0; i < Head.FileCount; i++)
            Data[i] = br.ReadArray<byte>(Entries[i].CompressedSize);
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        bw.Write(Head);
        bw.WriteArray(Entries, LengthPrefix.None);

        for (int i = 0; i < Data.Length; i++)
            bw.WriteArray(Data[i], LengthPrefix.None);
    }

    public struct MHead
    {
        public String8 Magic;
        public uint FileCount;
    }

    public struct MFileEntry
    {
        public String48 FileName;
        public uint Position, CompressedSize, TotalSize, Empty;
    }
}

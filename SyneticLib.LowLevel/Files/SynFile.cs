using System;
using System.IO;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grille.IO;
using SyneticLib.Files.Common;
using SyneticLib.LowLevel.Compression;

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

    public static void ExtractToDirectory(string srcFileName, string dstDirectoryName)
    {
        var syn = new SynFile();
        syn.Load(srcFileName);
        syn.ExtractToDirectory(dstDirectoryName);
    }

    public void ExtractToDirectory(string dstDirectoryName)
    {
        int length = Entries.Length;

        for (int i = 0; i < length; i++)
        {
            var head = Entries[i];
            var src = Data[i];
            var dst = new byte[head.TotalSize];

            SynDecompressor.Decompress(src, dst);
            var dstPath = Path.Combine(dstDirectoryName, head.FileName);
            File.WriteAllBytes(dstPath, dst);
        }
    }

    public static void ExtractFilesInDirectory(string dirPath, bool recursive, bool removeSynFiles)
    {
        foreach (var file in Directory.GetFiles(dirPath))
        {
            if (Path.GetExtension(file).ToLower() == ".syn")
            {
                ExtractToDirectory(file, dirPath);

                if (removeSynFiles)
                {
                    File.Delete(file);
                }
            }
        }

        if (recursive)
        {
            foreach (var dir in Directory.GetDirectories(dirPath))
            {
                ExtractFilesInDirectory(dir, recursive, removeSynFiles);
            }
        }
    }

    public override void Deserialize(BinaryViewReader br)
    {
        Head = br.Read<MHead>();
        br.Position += 52;

        Entries = br.ReadArray<MFileEntry>(Head.FileCount);

        Data = new byte[Head.FileCount][];

        for (int i = 0; i < Head.FileCount; i++)
        {
            Data[i] = br.ReadArray<byte>(Entries[i].CompressedSize);
        }
    }

    public override void Serialize(BinaryViewWriter bw)
    {
        throw new NotImplementedException();
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

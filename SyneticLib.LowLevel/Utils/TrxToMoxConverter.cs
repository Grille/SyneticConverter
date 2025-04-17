using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.Files.Extra;

using SyneticLib.Files;

namespace SyneticLib.Utils;

public static class TrxToMoxConverter
{
    public static void Convert(string dirPath, bool deleteTrx = true)
    {
        foreach (var file in Directory.EnumerateFiles(dirPath))
        {
            var ext = Path.GetExtension(file).ToLower();
            if (ext == ".trx")
            {
                var trx = new TrxFile();
                var mox = new MoxFile();

                trx.Load(file);

                Convert(trx, mox);

                mox.Save(Path.ChangeExtension(file, ".mox"));
                if (deleteTrx)
                {
                    File.Delete(file);
                }
            }
        }
    }

    public static void Convert(TrxFile trx, MoxFile mox)
    {
        mox.Head.Magic = (String4)"!XOM";
        mox.Head.Version.V0IndexMode = 0;
        mox.Head.Version.V1 = 255;
        mox.Head.Version.V2Extension = 1;
        mox.Head.Version.V3ChunkMode = 0;

        mox.Head.VtxCount = trx.Head.VtxCount;
        mox.Head.PolyCount = trx.Head.PolyCount;
        mox.Head.PaintRegionCount = trx.Head.PaintRegionCount;
        mox.Head.MatCount = trx.Head.MatCount;

        mox.Vertecis = trx.Vertecis;
        mox.Triangles = trx.Triangles;
        mox.Materials = trx.Materials;

        var obj = new WavefrontObjFile();
        obj.Vertecis = mox.Vertecis;
        obj.Triangles = mox.Triangles;

        mox.PaintRegions = new MoxFile.MPaintRegionInt32[trx.PaintRegions.Length];
        for (int i = 0; i < trx.PaintRegions.Length; i++)
        {
            ref var src = ref trx.PaintRegions[i];
            ref var dst = ref mox.PaintRegions[i];

            dst.MatId = src.SidA;
            dst.Clear0 = src.SidB;

            dst.VertBegin = src.VtxOffset;
            dst.VertEnd = src.VtxOffset + src.VtxCount - 1;

            if (dst.VertEnd >= mox.Vertecis.Length)
            {
                throw new InvalidDataException();
            }

            dst.PolyOffset = src.IdxOffset / 3;
            dst.PolyCount = src.PolyCount;
        }
    }
}

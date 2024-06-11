using SyneticLib.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Conversion;

public static class C11ToWR2FileConv
{
    public static void Convert(string path)
    {
        ConvertGeo(path);
        ConvertQad(path);
    }

    public static void ConvertGeo(string path)
    {
        string qeopath = Path.ChangeExtension(path, "geo");
        string idxpath = Path.ChangeExtension(path, "idx");
        string vtxpath = Path.ChangeExtension(path, "vtx");

        var geo = new GeoFile();
        var idx = new IdxFile();
        var vtx = new VtxFile();

        geo.Path = qeopath;
        idx.Path = idxpath;
        vtx.Path = vtxpath;

        geo.Load();

        idx.Indices = geo.Indices;
        vtx.Vertecis = geo.Vertecis;
        vtx.IndicesOffset = geo.IndicesOffset;

        idx.Save();
        vtx.Save();

        File.Delete(qeopath);
    }

    public static void ConvertQad(string path)
    {
        string qadpath = Path.ChangeExtension(path, "qad");

        var qad = new QadFile();

        qad.Path = qadpath;

        qad.SetFlagsAccordingToVersion(GameVersion.C11);
        qad.Load();

        qad.MaterialsWR = new QadFile.MMaterialTypeWR[qad.Head.MaterialCount];
        for (int i = 0; i < qad.Head.MaterialCount; i++)
        {
            ref var src = ref qad.MaterialsCT[i];
            ref var dst = ref qad.MaterialsWR[i];

            ConvertC11ToWR2(ref src, ref dst);
        }

        qad.SetFlagsAccordingToVersion(GameVersion.WR2);
        qad.SortMaterials();
        qad.RecalcMaterialChecksum();
        qad.Save();
    }

    public static void ConvertC11ToWR2(ref this QadFile.MMaterialTypeCT src, ref QadFile.MMaterialTypeWR dst)
    {
        dst.Mode = TerrainMaterialTypeWR2.UV;

        dst.Tex0Id = src.L1Tex0Id;
        dst.Tex1Id = src.L1Tex1Id;
        dst.Tex2Id = src.L1Tex2Id;

        dst.Matrix0 = src.Mat1;
        dst.Matrix1 = src.Mat1;
        dst.Matrix2 = src.Mat1;
    }
}

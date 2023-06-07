using SyneticLib.IO.Synetic.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticPipelineTool;

public static class C11ToWR2Conv
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

        idx.Polygons = geo.Polygons;
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

        qad.SetFlagsAccordingToVersion(SyneticLib.GameVersion.C11);
        qad.Load();

        qad.SortMaterials();
        qad.RecalcMaterialChecksum();
        qad.SetFlagsAccordingToVersion(SyneticLib.GameVersion.WR2);
        qad.Save();
    }
}

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.IO;

namespace SyneticLib.Files;

public class ScenarioFiles
{
    public SynFile Syn; 
    public GeoFile Geo; 
    public IdxFile Idx; 
    public LvlFile Lvl; 
    public SniFile Sni; 
    public VtxFile Vtx; 
    public QadFile Qad;
    public SkyFile Sky;

    public IVertexData VertexData;
    public IIndexData IndexData;

    public ScenarioFiles()
    {
        Syn = new SynFile();
        Geo = new GeoFile();
        Idx = new IdxFile();
        Lvl = new LvlFile();
        Sni = new SniFile();
        Vtx = new VtxFile();
        Qad = new QadFile();
        Sky = new SkyFile();

        VertexData = Vtx;
        IndexData = Idx;
    }

    public void SetPath(string dirPath, string fileName)
    {
        var filePath = Path.Combine(dirPath, fileName);
        Geo.Path = filePath + ".geo";
        Idx.Path = filePath + ".idx";
        Lvl.Path = filePath + ".lvl";
        Sni.Path = filePath + ".sni";
        Vtx.Path = filePath + ".vtx";
        Qad.Path = filePath + ".qad";
        Sky.Path = filePath + ".sky";
    }

    public void Deserialize(string dirPath, string fileName)
    {
        SetPath(dirPath, fileName);

        var version = Qad.FindGameVersion();
        Geo.SetFlagsAccordingToVersion(version);

        if (version >= GameVersion.C11)
        {
            Geo.Load();
            VertexData = Geo;
            IndexData = Geo;
        }
        else
        {
            Idx.Load();
            Vtx.Load();
            IndexData = Idx;
            VertexData = Vtx;
        }

        Lvl.Load();
        Qad.Load();

        if (Sni.Exists)
            Sni.Load();

        if (Sky.Exists)
            Sky.Load();
    }
}

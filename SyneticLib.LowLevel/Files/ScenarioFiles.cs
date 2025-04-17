using System.Diagnostics.CodeAnalysis;
using System.IO;

using SyneticLib.Files.Common;

using static SyneticLib.Files.ScenarioFiles;

namespace SyneticLib.Files;

public class ScenarioFiles
{
    public SynFile Syn; 
    public LvlFile Lvl; 
    public SniFile Sni; 
    public QadFile Qad;
    public SkyFile Sky;

    public Ro0File Ro1;
    public Ro0File Ro2;
    public Ro0File Ro3;
    public Ro0File Ro4;

    // Access through TerrainMesh
    private GeoFile Geo;
    private VtxFile Vtx;
    private IdxFile Idx;

    public readonly TerrainMeshView TerrainMesh;

    public class TerrainMeshView
    {
        [AllowNull] public Vertex[] Vertices;
        [AllowNull] public IdxTriangleInt32[] Indices;
        [AllowNull] public int[] Offsets;

        internal TerrainMeshView() { }
    }

    public class Paths
    {
        public string DirPath;

        public string GeoPath;
        public string IdxPath;
        public string LvlPath;
        public string SniPath;
        public string VtxPath;
        public string QadPath;
        public string SkyPath;

        public string Ro1Path;
        public string Ro2Path;
        public string Ro3Path;
        public string Ro4Path;

        public Paths(string dirPath, string fileName)
        {
            DirPath = dirPath;

            var filePath = Path.Combine(dirPath, fileName);
            GeoPath = filePath + ".geo";
            IdxPath = filePath + ".idx";
            LvlPath = filePath + ".lvl";
            SniPath = filePath + ".sni";
            VtxPath = filePath + ".vtx";
            QadPath = filePath + ".qad";
            SkyPath = filePath + ".sky";

            Ro1Path = filePath + ".ro1";
            Ro2Path = filePath + ".ro2";
            Ro3Path = filePath + ".ro3";
            Ro4Path = filePath + ".ro4";
        }
    }

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

        Ro1 = new Ro0File();
        Ro2 = new Ro0File();
        Ro3 = new Ro0File();
        Ro4 = new Ro0File();

        TerrainMesh = new TerrainMeshView();
        RefreshTerrainMesh(GameVersion.WR2);
    }

    public ScenarioFiles(string dirPath, string fileName) : this()
    {
        Load(dirPath, fileName);
    }

    public ScenarioFiles(string dirPath, string fileName, GameVersion version) : this()
    {
        Load(dirPath, fileName, version);
    }

    public void Load(string dirPath, string fileName)
    {
        var paths = new Paths(dirPath, fileName);

        var version = Qad.FindGameVersion(paths.QadPath);

        Load(paths, version);
    }

    public void Load(string dirPath, string fileName, GameVersion version)
    {
        var paths = new Paths(dirPath, fileName);

        Load(paths, version);
    }

    public void Load(Paths paths, GameVersion version)
    {
        Qad.SetFlagsAccordingToVersion(version);
        Qad.Load(paths.QadPath);

        Lvl.Load(paths.LvlPath);

        LoadTerrainMesh(paths, version);

        if (File.Exists(paths.Ro1Path))
        {
            Ro1.Load(paths.Ro1Path);
        }
        if (File.Exists(paths.Ro2Path))
        {
            Ro2.Load(paths.Ro2Path);
        }
        if (File.Exists(paths.Ro3Path))
        {
            Ro3.Load(paths.Ro3Path);
        }
        if (File.Exists(paths.Ro4Path))
        {
            Ro4.Load(paths.Ro4Path);
        }

        if (File.Exists(paths.SniPath))
        {
            Sni.Load(paths.SniPath);
        }

        if (File.Exists(paths.SkyPath))
        {
            Sky.Load(paths.SkyPath);
        }
    }

    public void LoadTerrainMesh(string dirPath, string fileName, GameVersion version)
    {
        var paths = new Paths(dirPath,  fileName);
        LoadTerrainMesh(paths, version);
    }

    public void LoadTerrainMesh(Paths paths, GameVersion version)
    {
        if (version >= GameVersion.C11)
        {
            Geo.Load(paths.GeoPath);
        }
        else
        {
            Idx.Load(paths.IdxPath);
            Vtx.Load(paths.VtxPath);
        }

        RefreshTerrainMesh(version);
    }

    public void SaveTerrainMesh(string dirPath, string fileName, GameVersion version)
    {
        var paths = new Paths(dirPath, fileName);
        SaveTerrainMesh(paths, version);
    }

    private void RefreshTerrainMesh(GameVersion version)
    {
        if (version >= GameVersion.C11)
        {
            TerrainMesh.Vertices = Geo.Vertecis;
            TerrainMesh.Indices = Geo.Triangles;
            TerrainMesh.Offsets = Geo.IndicesOffset;
        }
        else
        {
            TerrainMesh.Vertices = Vtx.Vertecis;
            TerrainMesh.Indices = Idx.Triangles;
            TerrainMesh.Offsets = Vtx.IndicesOffset;
        }
    }

    private void FlushTerrainMesh(GameVersion version)
    {
        if (version >= GameVersion.C11)
        {
            Geo.Vertecis = TerrainMesh.Vertices;
            Geo.IndicesOffset = TerrainMesh.Offsets;
            Geo.Triangles = TerrainMesh.Indices;
        }
        else
        {
            Vtx.Vertecis = TerrainMesh.Vertices;
            Vtx.IndicesOffset = TerrainMesh.Offsets;
            Idx.Triangles = TerrainMesh.Indices;
        }
    }

    public void SaveTerrainMesh(Paths paths, GameVersion version)
    {
        FlushTerrainMesh(version);

        if (version >= GameVersion.C11)
        {
            Geo.Save(paths.GeoPath);
        }
        else
        {
            Idx.Save(paths.IdxPath);
            Vtx.Save(paths.VtxPath);
        }
    }

    public void Save(string dirPath, string fileName, GameVersion version)
    {
        var paths = new Paths(dirPath, fileName);
        Save(paths, version);
    }

    public void Save(Paths paths, GameVersion version)
    {
        Qad.SetFlagsAccordingToVersion(version);
        Qad.Save(paths.QadPath);

        SaveTerrainMesh(paths, version);

        Sni.Save(paths.SniPath);

        if (Ro1.Head.GrassLength > 0)
        {
            Ro1.Save(paths.Ro1Path);
        }
        if (Ro2.Head.GrassLength > 0)
        {
            Ro2.Save(paths.Ro2Path);
        }
        if (Ro3.Head.GrassLength > 0)
        {
            Ro3.Save(paths.Ro3Path);
        }
        if (Ro4.Head.GrassLength > 0)
        {
            Ro4.Save(paths.Ro4Path);
        }

        if (version == GameVersion.WR2)
        {
            Sky.Save(paths.SkyPath);
        }
    }
}

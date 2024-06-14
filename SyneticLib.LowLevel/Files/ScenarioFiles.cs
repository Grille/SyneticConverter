﻿using System.IO;

using SyneticLib.Files.Common;

namespace SyneticLib.Files;

public class ScenarioFiles
{
    public SynFile Syn; 
    public LvlFile Lvl; 
    public SniFile Sni; 
    public QadFile Qad;
    public SkyFile Sky;

    // Access through TerrainMesh
    private GeoFile Geo;
    private VtxFile Vtx;
    private IdxFile Idx;

    public TerrainMeshObj TerrainMesh;

    public struct TerrainMeshObj
    {
        public Vertex[] Vertices;
        public IdxTriangleInt32[] Indices;
        public int[] Offsets;
    }

    public class Paths
    {
        public string GeoPath;
        public string IdxPath;
        public string LvlPath;
        public string SniPath;
        public string VtxPath;
        public string QadPath;
        public string SkyPath;

        public Paths(string dirPath, string fileName)
        {
            var filePath = Path.Combine(dirPath, fileName);
            GeoPath = filePath + ".geo";
            IdxPath = filePath + ".idx";
            LvlPath = filePath + ".lvl";
            SniPath = filePath + ".sni";
            VtxPath = filePath + ".vtx";
            QadPath = filePath + ".qad";
            SkyPath = filePath + ".sky";
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

        if (File.Exists(paths.SniPath))
            Sni.Load(paths.SniPath);

        if (File.Exists(paths.SkyPath))
            Sky.Load(paths.SkyPath);
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
            Geo.SetFlagsAccordingToVersion(version);
            Geo.Load(paths.GeoPath);
            TerrainMesh.Vertices = Geo.Vertecis;
            TerrainMesh.Indices = Geo.Indices;
            TerrainMesh.Offsets = Geo.IndicesOffset;
        }
        else
        {
            Idx.Load(paths.IdxPath);
            Vtx.Load(paths.VtxPath);
            TerrainMesh.Vertices = Vtx.Vertecis;
            TerrainMesh.Indices = Idx.Indices;
            TerrainMesh.Offsets = Vtx.IndicesOffset;
        }
    }

    public void SaveTerrainMesh(string dirPath, string fileName, GameVersion version)
    {
        var paths = new Paths(dirPath, fileName);
        SaveTerrainMesh(paths, version);
    }

    public void SaveTerrainMesh(Paths paths, GameVersion version)
    {
        if (version >= GameVersion.C11)
        {
            Geo.Vertecis = TerrainMesh.Vertices;
            Geo.IndicesOffset = TerrainMesh.Offsets;
            Geo.Indices = TerrainMesh.Indices;
            Geo.SetFlagsAccordingToVersion(version);
            Geo.Save(paths.GeoPath);
        }
        else
        {
            Vtx.Vertecis = TerrainMesh.Vertices;
            Vtx.IndicesOffset = TerrainMesh.Offsets;
            Idx.Indices = TerrainMesh.Indices;
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

        if (version == GameVersion.WR2)
        {
            Sky.Save(paths.SkyPath);
        }
    }
}

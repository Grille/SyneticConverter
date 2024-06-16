using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

using SyneticLib.Files;

namespace SyneticLib;

public class ScenarioChunk : SyneticObject
{
    public readonly int X, Z;
    public Model Terrain { get; }
    public Vector3 Center { get; }
    public float Radius { get; }

    public ScenarioChunk(in ScenarioChunkCreateInfo createInfo)
    {
        X = createInfo.Position.X;
        Z = createInfo.Position.Z;
        Terrain = createInfo.Terrain.GetModel(X, Z);
        Radius = createInfo.Radius;
        Center = createInfo.Center;
    }
}

public ref struct ScenarioChunkCreateInfo
{
    public Location2DUInt16 Position;
    public TerrainModel Terrain;
    public Vector3 Center;
    public float Radius;

    public ScenarioChunkCreateInfo(in QadFile.MChunk mChunk)
    {
        Position = mChunk.Position;
        Center = mChunk.Center;
        Radius = mChunk.Radius;
        Unsafe.SkipInit(out Terrain);
    }
}

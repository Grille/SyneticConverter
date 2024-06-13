using OpenTK.Mathematics;
using SyneticLib.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Math3D;

namespace SyneticLib.Graphics.DrawCalls;

internal class ScenarioGlHandle : IDisposable
{
    readonly int _width;

    readonly int _height;

    public MeshBuffer Terrain { get; }

    GlObjectCacheGroup _cache;

    ModelDrawCalls[,] TerrainDrawCalls;

    public ScenarioGlHandle(Scenario scenario)
    {
        _width = scenario.Width;
        _height = scenario.Height;

        _cache = new GlObjectCacheGroup();

        Terrain = _cache.Meshes.GetGlObject(scenario.Terrain);

        TerrainDrawCalls = new ModelDrawCalls[scenario.Width, scenario.Height];

        for (int iy = 0; iy < _height; iy++)
        {
            for (int ix = 0; ix < _width; ix++)
            {
                var srcChunk = scenario.Chunks[ix, iy];
                var chunk = new ModelDrawCalls(srcChunk.Terrain, _cache);
                TerrainDrawCalls[ix, iy] = chunk;
            }
        }

        _cache.Uncouple();
    }

    public void SubCamera(Camera camera)
    {
        foreach (var item in _cache.Materials)
        {
            item.Bind();
            item.SubModelMatrix(Matrix4.Identity);
            item.SubCameraMatrix(camera);
        }
    }

    public void DrawChunk(int x, int y)
    {
        (int, int) Clamp(int value, int min, int max)
        {
            if (value < min)
                return (min, value);
            if (value >= max)
                return (max - 1, value - max);
            return (value, 0);
        }

        (int posX, int offsetX) = Clamp(x, 0, _width);
        (int posY, int offsetY) = Clamp(y, 0, _height);

        var matrix = Matrix4.CreateTranslation(offsetX * 1024, 0, offsetY * 1024);

        TerrainDrawCalls[posX, posY].Execute(matrix);
    }

    public void DrawScenario(Vector3 position, float radius)
    {
        float posX = position.X / 1024f + _width / 2;
        float posY = position.Z / 1024f + _height / 2;

        Console.WriteLine(posX);
        Console.WriteLine(posY);

        int beginX = (int)Math.Floor(posX - radius*2);
        int beginY = (int)Math.Floor(posY - radius*2);

        int endX = (int)Math.Floor(posX + radius*2);
        int endY = (int)Math.Floor(posY + radius*2);

        Terrain.Bind();

        var rDistSq = radius * radius + radius * radius;

        for (int iy = beginY; iy < endY; iy++)
        {
            for (int ix = beginX; ix < endX; ix++)
            {
                var distX = MathF.Abs(ix - posX);
                var distY = MathF.Abs(iy - posY);

                var distSq = distX * distX + distY * distY;

                if (distSq > rDistSq)
                    continue;

                DrawChunk(ix, iy);
            }
        }
    }

    public void DrawScenario()
    {
        Terrain.Bind();
        
        for (int iy = 0; iy < _height; iy++)
        {
            for (int ix = 0; ix < _width; ix++)
            {
                DrawChunk(ix, iy);
            }
        }

        //new DrawElementsInfo(0, ushort.MaxValue, 0).Excecute();
    }

    public void Dispose()
    {
        Terrain.Dispose();
        _cache.Dispose();
    }
}

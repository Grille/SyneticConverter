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

    public void DrawScenario(Vector3 position, float radius)
    {
        DrawScenario(new Vector2(position.X, position.Y), radius);
    }

    public void DrawScenario(Vector2 position, float radius)
    {
        float x = position.X;
        float y = position.Y;

        int beginX = (int)Math.Floor(x - radius);
        int beginY = (int)Math.Floor(y - radius);

        int endX = (int)Math.Floor(x + radius);
        int endY = (int)Math.Floor(y + radius);

        for (int iy = 0; iy < _height; iy++)
        {
            for (int ix = 0; ix < _width; ix++)
            {
                TerrainDrawCalls[ix, iy].Execute();
            }
        }

        return;
        for (int iy = beginX; iy < endX; iy++)
        {
            for (int ix = beginY; ix < endY; ix++)
            {

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
                TerrainDrawCalls[ix, iy].Execute();
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

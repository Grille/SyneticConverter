using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

using SyneticLib.Graphics.Materials;
using SyneticLib.Graphics.OpenGL;
using SyneticLib.Math3D;

namespace SyneticLib.Graphics.DrawCalls;

public class TerrainGlHandle : IDisposable
{
    readonly int _width;

    readonly int _height;

    public float GridSize = 1024f;

    public MeshBuffer Terrain { get; }

    GlObjectCacheGroup _cache;

    ModelDrawCalls[,] TerrainDrawCalls;

    public TerrainGlHandle(TerrainModel terrain)
    {
        _width = terrain.Width;
        _height = terrain.Height;

        _cache = new GlObjectCacheGroup();

        Terrain = _cache.Meshes.GetGlObject(terrain.Mesh);

        TerrainDrawCalls = new ModelDrawCalls[terrain.Width, terrain.Height];

        for (int iy = 0; iy < _height; iy++)
        {
            for (int ix = 0; ix < _width; ix++)
            {
                var model = terrain.GetModel(ix, iy);
                var chunk = new ModelDrawCalls(model, _cache);
                TerrainDrawCalls[ix, iy] = chunk;
            }
        }

        _cache.Uncouple();
    }

    public void DrawChunk(int x, int y, bool drawBackground = false)
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

        if (!drawBackground && (offsetX != 0 || offsetY != 0))
            return;

        var matrix = Matrix4.CreateTranslation(offsetX * 1024, 0, offsetY * 1024);

        if (GLContext.UsedProgram is TerrainMaterialProgram tm)
        {
            tm.SubModelMatrix(matrix);
        }

        TerrainDrawCalls[posX, posY].Execute();
    }

    public void Draw(Vector3 position, float radius)
    {
        float posX = position.X / GridSize + _width / 2;
        float posY = position.Z / GridSize + _height / 2;

        int beginX = (int)Math.Floor(posX - radius * 2);
        int beginY = (int)Math.Floor(posY - radius * 2);

        int endX = (int)Math.Floor(posX + radius * 2);
        int endY = (int)Math.Floor(posY + radius * 2);

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

    public void Draw()
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace SyneticLib;
public class TerrainModel
{
    public int Width { get; }
    public int Height { get; }
    public int Length { get; }

    public Mesh Mesh { get; }

    readonly Model[,] _chunks;

    public TerrainModel(Mesh mesh, Model[,] chunks)
    {
        Width = chunks.GetLength(0);
        Height = chunks.GetLength(1);
        Length = chunks.Length;
        Mesh = mesh;
        _chunks = chunks;
        FillChunks(chunks);
    }

    void FillChunks(Model[,] chunks)
    {
        for (int iy = 0; iy < Height; iy++)
        {
            for (int ix = 0; ix < Width; ix++)
            {
                var chunk = _chunks[ix, iy];
                if (chunk.MeshSection.Mesh != Mesh)
                {
                    throw new ArgumentException();
                }
                _chunks[ix,iy] = chunks[ix, iy];
            }
        }
    }

    public static TerrainModel CreateTerrainFromModel(Model model)
    {
        var boundings = model.BoundingBox;

        Vector3 size = boundings.Size;
        Vector3 offset = boundings.Start;

        return null;
    }

    public Model GetModel(int x, int y)
    {
        return _chunks[x, y];
    }
}

﻿using OpenTK.Mathematics;
using SyneticLib.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Math3D;
using SyneticLib.World;

namespace SyneticLib.Graphics.DrawCalls;

internal class ScenarioGlHandle : IDisposable
{
    readonly int _width;

    readonly int _height;

    public TerrainGlHandle Terrain { get; }

    GlObjectCacheGroup _cache;

    public ScenarioGlHandle(Scenario scenario)
    {
        _width = scenario.Width;
        _height = scenario.Height;

        _cache = new GlObjectCacheGroup();

        Terrain = new TerrainGlHandle(scenario.Terrain);

        _cache.Uncouple();
    }

    public void DrawScenario(Vector3 position, float radius)
    {
        Terrain.Draw(position, radius);
    }

    public void DrawScenario()
    {
        Terrain.Draw();
    }

    public void Dispose()
    {
        Terrain.Dispose();
        _cache.Dispose();
    }
}

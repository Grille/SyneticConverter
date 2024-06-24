using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

using SyneticLib.Math3D;

using SyneticLib.Graphics.OpenGL;
using SyneticLib.Graphics.Materials;

namespace SyneticLib.Graphics.DrawCalls;

public class ModelGlHandle : IDisposable
{
    readonly GlObjectCacheGroup _cache;

    bool _disposeCache;

    public MeshBuffer Mesh { get; }

    public ModelDrawCalls DrawCalls { get; }

    public ModelGlHandle(Model model) : this(model, new GlObjectCacheGroup())
    {
        _disposeCache = true;
    }

    public ModelGlHandle(Model model, GlObjectCacheGroup cache)
    {
        _cache = cache;

        Mesh = cache.Meshes.GetGlObject(model.MeshSection.Mesh);
        DrawCalls = new ModelDrawCalls(model, cache);
    }

    public void DrawModel()
    {
        Mesh.Bind();
        DrawCalls.Execute();
    }

    public void Dispose()
    {
        if (_disposeCache)
        {
            _cache.Dispose();
        }
    }
}

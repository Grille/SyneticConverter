using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

using SyneticLib.Math3D;

using SyneticLib.Graphics.OpenGL;

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

    public void SubCamera(Camera camera)
    {
        foreach (var item in _cache.Materials)
        {
            item.Bind();
            item.SubModelMatrix(Matrix4.Identity);
            item.SubCameraMatrix(camera);
        }
    }

    public void DrawModel(Matrix4 modelMatrix)
    {
        Mesh.Bind();
        DrawCalls.Execute(modelMatrix);
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

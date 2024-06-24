using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;
using SyneticLib.Graphics.Materials;
using SyneticLib.Graphics.OpenGL;

namespace SyneticLib.Graphics.DrawCalls;

public struct ModelDrawCalls
{
    public record struct DrawCall(DrawElementsInfo DrawInfo, MaterialUniforms Uniforms);

    readonly DrawCall[] _drawCalls;

    public ModelDrawCalls(Model model, GlObjectCache<Material, MaterialUniforms> cache)
    {
        _drawCalls = new DrawCall[model.MaterialRegions.Length];

        for (int i = 0; i < model.MaterialRegions.Length; i++)
        {
            var region = model.MaterialRegions[i];
            var program = cache.GetGlObject(region.Material);
            var drawInfo = new DrawElementsInfo(region.ElementStart * 3, region.ElementCount * 3, model.MeshSection.Offset);
            var call = new DrawCall(drawInfo, program);
            _drawCalls[i] = call;
        }
    }

    public ModelDrawCalls(Model model, GlObjectCacheGroup cache) : this(model, cache.Materials) { }

    public void Execute()
    {
        for (int i = 0; i < _drawCalls.Length; i++)
        {
            ref var call = ref _drawCalls[i];
            call.Uniforms.Bind();
            call.DrawInfo.Excecute();
        }
    }
}

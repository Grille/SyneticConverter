using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;
using SyneticLib.Graphics.OpenGL;

namespace SyneticLib.Graphics.DrawCalls;

public struct ModelDrawCalls
{
    public record struct DrawCall(DrawElementsInfo DrawInfo, MaterialProgram Program);

    readonly DrawCall[] _drawCalls;

    public ModelDrawCalls(Model model, GlObjectCache<Material, MaterialProgram> cache)
    {
        _drawCalls = new DrawCall[model.MaterialRegions.Length];

        for (int i = 0; i < model.MaterialRegions.Length; i++)
        {
            var region = model.MaterialRegions[i];
            var program = cache.GetGlObject(region.Material);
            program.Bind();
            var drawInfo = new DrawElementsInfo(region.ElementStart * 3, region.ElementCount * 3, model.MeshSection.Offset);
            var call = new DrawCall(drawInfo, program);
            _drawCalls[i] = call;
        }
    }

    public ModelDrawCalls(Model model, GlObjectCacheGroup cache) : this(model, cache.Materials) { }

    public void Execute(Matrix4 modelMatrix)
    {
        for (int i = 0; i < _drawCalls.Length; i++)
        {
            ref var call = ref _drawCalls[i];
            call.Program.BindEnabledTextures();
            call.Program.Bind();
            call.Program.SubModelMatrix(modelMatrix);
            call.DrawInfo.Excecute();
        }
    }

    public void Execute()
    {
        Execute(Matrix4.Identity);
    }
}

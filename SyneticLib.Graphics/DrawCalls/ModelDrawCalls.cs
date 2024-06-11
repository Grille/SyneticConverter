using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;
using SyneticLib.Graphics.OpenGL;

namespace SyneticLib.Graphics.DrawCalls;

public class ModelDrawCalls
{
    public record struct DrawCall(DrawElementsInfo DrawInfo, MaterialProgram Program);

    public DrawCall[] DrawCalls;

    public ModelDrawCalls(Model model, GlObjectCache<Material, MaterialProgram> cache)
    {
        DrawCalls = new DrawCall[model.MaterialRegions.Length];

        for (int i = 0; i < model.MaterialRegions.Length; i++)
        {
            var region = model.MaterialRegions[i];
            var program = cache.GetGlObject(region.Material);
            program.Bind();
            var drawInfo = new DrawElementsInfo(region.ElementStart * 3, region.ElementCount * 3, model.MeshSection.Offset);
            var call = new DrawCall(drawInfo, program);
            DrawCalls[i] = call;
        }
    }

    public ModelDrawCalls(Model model, GlObjectCacheGroup cache) : this(model, cache.Materials) { }

    public void Execute(Matrix4 modelMatrix)
    {
        for (int i = 0; i < DrawCalls.Length; i++)
        {
            ref var call = ref DrawCalls[i];
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

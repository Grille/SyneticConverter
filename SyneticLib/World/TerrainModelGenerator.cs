using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.World;


public unsafe class TerrainModelGenerator
{
    record struct TrianglePtr(IdxTriangleInt32 Triangle, Material Material);

    public void DoStuff(Model model)
    {
        var materialsSet = new HashSet<Material>();

        var srcVertices = model.MeshSection.Vertices;
        var srcIndices = model.MeshSection.Indices;

        var triangles = new TrianglePtr[srcIndices.Length];

        var iDst = 0;
        for (var i = 0; i < model.MaterialRegions.Length; i++)
        {
            var region = model.MaterialRegions[i];
            var material = model.MaterialRegions[i].Material;

            for (var iSrc = 0; iSrc < region.ElementCount; iSrc++)
            {
                triangles[iDst++] = new TrianglePtr(srcIndices[iSrc + region.ElementStart], material);
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class Model : SyneticObject
{
    public MeshSegment MeshSection { get; }

    public Material[] Materials { get; }

    public ModelMaterialRegion[] MaterialRegions { get; }

    public Model[]? SubSections { get; }

    public BoundingBox BoundingBox => MeshSection.BoundingBox;

    public Model(MeshSegment mesh, ModelMaterialRegion[] regions)
    {
        MeshSection = mesh;
        MaterialRegions = regions;

        var materials = new List<Material>();
        foreach (var region in regions)
        {
            var material = region.Material;
            if (materials.Contains(material))
                continue;

            materials.Add(material);
        }

        Materials = materials.ToArray();
    }

    public Model(MeshSegment mesh, Material material) :
        this(mesh, new[] { new ModelMaterialRegion(0, mesh.Length, material) })
    { }
}

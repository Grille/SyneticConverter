using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class Model : SyneticObject
{
    public MeshSegment MeshSection { get; }

    public ModelMaterialRegion[] MaterialRegions { get; }

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
    }

    public Model(MeshSegment mesh, Material material) :
        this(mesh, new[] { new ModelMaterialRegion(0, mesh.Length, material) })
    { }

    public void GetMaterials(ISet<Material> set)
    {
        foreach (var region in MaterialRegions)
        {
            set.Add(region.Material);
        }
    }

    public HashSet<Material> GetMaterials()
    {
        var set = new HashSet<Material>();
        GetMaterials(set);
        return set;
    }

    public void GetTextures(ISet<Texture> set)
    {
        var materials = GetMaterials();
        foreach (var material in materials)
        {
            material.GetTextures(set);
        }
    }

    public HashSet<Texture> GetTextures()
    {
        var set = new HashSet<Texture>();
        GetTextures(set);
        return set;
    }
}

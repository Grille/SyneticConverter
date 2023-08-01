using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class Model : Ressource
{
    public Mesh Mesh { get; }

    public Material[] Materials { get; }

    public ModelMaterialRegion[] MaterialRegions { get; }

    public Model(string name, Mesh mesh, ModelMaterialRegion[] regions) : base(name)
    {
        Mesh = mesh;
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

    public Model(string name, Mesh mesh, Material material) :
        this(name, mesh, new[] { new ModelMaterialRegion(0, mesh.Indices.Length, material) })
    { }
}

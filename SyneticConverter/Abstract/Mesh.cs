using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

namespace SyneticConverter;

public class Mesh
{
    public string Name;
    public int[] Indecies;
    public Vertex[] Vertices;
    public Vector3Int[] Poligons;
    public PolyRegion[] PolyRegion;
    public MaterialList Materials;

    public Mesh(MaterialList materials)
    {
        Materials = materials;
    }

    public void ImportMox(string path)
    {
        var imp = new MeshImporterMox(this);
        imp.Load(path);
        imp.Assign();
    }

    public void ExportObj(string path, bool fix = true)
    {
        var exp = new MeshExporterObj(this);
        if (fix)
            exp.PositionMultiplier = new Vector3(-0.1f, 0.1f, 0.1f);

        exp.Save(path);
    }

    public void ExportSbi(string path)
    {
        var exp = new MeshExporterSbi(this);
        exp.Save(path);
    }
}

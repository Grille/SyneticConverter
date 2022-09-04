using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

using SyneticLib.IO.Synetic;
using SyneticLib.IO.Extern;
using SyneticLib.Graphics;

namespace SyneticLib;

public abstract class Mesh: Ressource
{
    public string Name;
    //public int[] Indecies;
    public Vertex[] Vertices;
    public Vector3Int[] Poligons;
    public MaterialRegion[] MaterialRegion;

    public MaterialList Materials { get; set; }

    public GLMeshBuffer GLBuffer;

    public Mesh(Ressource parent, string path, PointerType type) : base(parent, path, type)
    {
        Materials = new(this);
    }

    public abstract void ExportAsObj(string path);

    public void ExportAsSbx(string path)
    {
        var exp = new MeshExporterSbx(this);
        exp.Save(path);
    }
}

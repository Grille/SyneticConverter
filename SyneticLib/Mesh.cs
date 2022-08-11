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

public class Mesh: Ressource
{
    public string Name;
    public int[] Indecies;
    public MeshVertex[] Vertices;
    public Vector3Int[] Poligons;
    public MaterialRegion[] MaterialRegion;
    public MaterialList Materials;

    public GLState GLState;
    public GpuMeshBuffer GLBuffer;

    public Mesh(MaterialList materials): base(null, PointerType.File)
    {
        Materials = materials;
        GLState = GLState.None;
        GLBuffer = new GpuMeshBuffer(this);
    }

    public void ImportFromMox(string path)
    {
        var imp = new MeshImporterMox(this);
        imp.Load(path);
        imp.Assign();
    }

    public void ExportAsObj(string path, bool fix = true)
    {
        var exp = new MeshExporterObj(this);
        if (fix)
            exp.PositionMultiplier = new Vector3(-0.1f, 0.1f, 0.1f);

        exp.Save(path);
    }

    public void ExportAsSbx(string path)
    {
        var exp = new MeshExporterSbx(this);
        exp.Save(path);
    }

    protected override void OnLoad()
    {
        throw new NotImplementedException();
    }

    protected override void OnSave()
    {
        throw new NotImplementedException();
    }

    protected override void OnSeek()
    {
        throw new NotImplementedException();
    }
}

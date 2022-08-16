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
    //public int[] Indecies;
    public MeshVertex[] Vertices;
    public Vector3Int[] Poligons;
    public MaterialRegion[] MaterialRegion;
    public MaterialList Materials;

    public MeshBuffer GLBuffer;

    public Mesh(Ressource parent, string path): base(parent, PointerType.File)
    {
        SourcePath = path;
        GLBuffer = new MeshBuffer(this);
    }

    public void ImportFromMox()
    {
        new MeshImporterMox(this).Load();
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
        switch (Extension.ToLower()) {
            case ".mox":
                ImportFromMox();
                return;
            default:
                throw new InvalidOperationException($"'{SourcePath}' is not a valid model file.");
    }
    }

    protected override void OnSave()
    {
        throw new NotImplementedException();
    }

    protected override void OnSeek()
    {
        //throw new NotImplementedException();
    }
}

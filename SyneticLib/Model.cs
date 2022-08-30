using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.IO.Synetic;
using SyneticLib.Graphics;

namespace SyneticLib;
public class Model : Mesh
{
    public Model(Ressource parent, string path) : base(parent, path, PointerType.File)
    {
        GLBuffer = new ModelBuffer(this);
    }

    public override void ExportAsObj(string path)
    {
        throw new NotImplementedException();
    }

    public void ImportFromMox()
    {
        new MeshImporterMox(this).Load();
    }

    protected override void OnLoad()
    {
        switch (FileExtension.ToLower())
        {
            case ".mox":
                ImportFromMox();
                return;
            default:
                throw new InvalidOperationException($"'{SourcePath}' is not a valid model file.");
        }
    }

    protected override void OnSave()
    {

    }

    protected override void OnSeek()
    {

    }
}

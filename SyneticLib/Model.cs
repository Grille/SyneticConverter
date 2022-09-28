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
    public TextureDirectory AssignedTextures;

    public Model() : base(null, "Model", PointerType.Virtual)
    {
        GLBuffer = new ModelBuffer(this);
    }

    public Model(Ressource parent, TextureDirectory textures, string path) : base(parent, path, PointerType.File)
    {
        GLBuffer = new ModelBuffer(this);
        AssignedTextures = textures;
    }

    public override void ExportAsObj(string path)
    {
        throw new NotImplementedException();
    }

    public void ImportFromMox()
    {
        new ModelmporterMox(this).Load();
    }

    protected override void OnLoad()
    {
        switch (FileExtension.ToLower())
        {
            case ".mox":
                ImportFromMox();
                break;
            default:
                throw new InvalidOperationException($"'{SourcePath}' is not a valid model file.");
        }

        foreach (var texture in AssignedTextures)
        {
            texture.Load();
        }
    }

    protected override void OnSave()
    {

    }

    protected override void OnSeek()
    {

    }
}

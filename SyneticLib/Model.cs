using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.IO.Synetic;

namespace SyneticLib;
public class Model : Ressource
{
    public Mesh Mesh { get; set; }

    public MaterialList Materials { get; set; }

    public ModelMaterialRegion[] MaterialRegions { get; set; }


    public Model() : base(null, "Model", PointerType.Virtual)
    {
    }

    public Model(Ressource parent, string path) : base(parent, path, PointerType.File)
    {
    }

    public void MaterialsFromRegions()
    {
        Materials.Clear();

        foreach (var region in MaterialRegions)
        {

        }
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

        foreach (var texture in Materials)
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

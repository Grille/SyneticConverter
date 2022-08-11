using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;

public class Scenario : Ressource
{
    public new GameFolder Parent { get => (GameFolder)base.Parent; set => base.Parent = value; }


    public List<ScenarioVariant> Variants;

    public int VariantCount { get => Variants.Count; }
    public ScenarioVariant V1 { get => Variants.Count > 0 ? Variants[0] : null; } 
    public ScenarioVariant V2 { get => Variants.Count > 1 ? Variants[1] : null; }
    public ScenarioVariant V3 { get => Variants.Count > 2 ? Variants[2] : null; }
    public ScenarioVariant V4 { get => Variants.Count > 3 ? Variants[3] : null; }

    public Scenario() : base(null, PointerType.Directory)
    {
        Variants = new List<ScenarioVariant>();
    }


    public Scenario(GameFolder game, string path) :base(game, PointerType.Directory)
    {
        SourcePath = path;

        InitStructure();
    }

    public void InitStructure()
    {
        var dirs = Directory.GetDirectories(SourcePath);

        var list = Variants;
        list.Clear();

        foreach (var dir in dirs)
        {
            string variantId = Path.GetFileName(dir);
            if (variantId.Length == 2 && int.TryParse(variantId.Substring(1, 1), out int result))
            {
                list.Add(new ScenarioVariant(this, result));
            }
        }

        foreach (var v in Variants)
        {
            v.PeakHead();
        }
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

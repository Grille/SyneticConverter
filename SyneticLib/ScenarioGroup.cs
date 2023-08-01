using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SyneticLib.Locations;

namespace SyneticLib;

public class ScenarioGroup : Ressource
{
    public List<Scenario> Variants;

    public int VariantCount { get => Variants.Count; }
    public Scenario V1 { get => Variants.Count > 0 ? Variants[0] : null; } 
    public Scenario V2 { get => Variants.Count > 1 ? Variants[1] : null; }
    public Scenario V3 { get => Variants.Count > 2 ? Variants[2] : null; }
    public Scenario V4 { get => Variants.Count > 3 ? Variants[3] : null; }

    public ScenarioGroup(GameDirectory game, string path) : base(path)
    {
        Variants = new List<Scenario>();
    }

    public void ExecuteOnAll(Action<Scenario> action)
    {
        foreach (var variant in Variants)
        {
            action(variant);
        }
    }

    public void CopyFilesTo(GameDirectory game)
    {
        ExecuteOnAll((v) =>
        {
        });
    }

    /*
    protected override void OnLoad()
    {
        foreach (var variant in Variants)
        {
            variant.Load();
        }
    }

    protected override void OnSave()
    {
        throw new NotImplementedException();
    }

    protected override void OnSeek()
    {
        var dirs = Directory.GetDirectories(SourcePath);

        var list = Variants;
        list.Clear();

        foreach (var dir in dirs)
        {
            string variantId = Path.GetFileName(dir);
            if (variantId.Length == 2 && int.TryParse(variantId.Substring(1, 1), out int result))
            {
                list.Add(new Scenario(this, result));
            }
        }
    }
    */
}

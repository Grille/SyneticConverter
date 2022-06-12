using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticConverter;

public class Scenario
{
    public readonly GameFolder Game;

    public string Name;
    public string RootDir;

    public ScenarioVariant[] Variants;

    public Scenario(GameFolder game, string path)
    {
        Game = game;
        Name = Path.GetFileName(path);
        RootDir = path;
        //if (!Directory.Exists(RootDir))
        //    return;

        var dirs = Directory.GetDirectories(RootDir);

        var list = new List<ScenarioVariant>();

        foreach (var dir in dirs)
        {
            string variantId = Path.GetFileName(dir);
            if (variantId.Length == 2 && int.TryParse(variantId.Substring(1, 1), out int result))
            {
                list.Add(new ScenarioVariant(this, result));
            }
        }
        Variants = list.ToArray();
    }

    public void LoadAllData()
    {
        foreach (var variant in Variants)
        {
            variant.LoadData();
        }
    }

    public void Save()
    {
        SaveAs(Name);
    }

    public void Delete()
    {

    }

    public void SaveAs(string name)
    {

    }

    public void SaveAs(string name, GameFolder game)
    {

    }

    public void SaveAs(string name, string rootPath, TargetFormat format)
    {

    }

}

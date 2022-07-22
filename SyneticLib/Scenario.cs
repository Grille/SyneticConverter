using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;

public class Scenario : Ressource
{
    public readonly GameFolder Game;

    public string Name;
    public string RootDir;

    public ScenarioVariant[] Variants;

    public InitState State { 
        get 
        {
            if (Variants.Length == 0)
                return InitState.Empty;

            InitState state = Variants[0].State;
            for (int i = 1; i < Variants.Length; i++)
            {
                if (Variants[i].State < state)
                    state = Variants[i].State;
            }
            return state;
        }
    }

    public int VCount { get => Variants.Length; }
    public ScenarioVariant V1 { get => Variants.Length > 0 ? Variants[0] : null; }
    public ScenarioVariant V2 { get => Variants.Length > 1 ? Variants[1] : null; }
    public ScenarioVariant V3 { get => Variants.Length > 2 ? Variants[2] : null; }
    public ScenarioVariant V4 { get => Variants.Length > 3 ? Variants[3] : null; }

    public override DataState DataState => throw new NotImplementedException();

    public Scenario(GameFolder game, string path)
    {
        Game = game;
        Name = Path.GetFileName(path);
        RootDir = path;

        InitStructure();
    }

    public void InitStructure()
    {
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

        foreach (var v in Variants)
        {
            v.PeakHead();
        }
    }

    public override void LoadAll()
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

    public void SaveAs(string name, string rootPath, GameVersion format)
    {

    }

    public override void CopyTo(string path)
    {
        throw new NotImplementedException();
    }

    public override void SeekAll()
    {
        throw new NotImplementedException();
    }
}

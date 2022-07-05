using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticConverter;

public partial class ScenarioVariant
{
    public readonly Scenario Owner;
    public int Number;
    public string RootDir;

    public int Width, Height;


    public GroundModel GroundModel;
    public List<Sound> Sounds;
    public TextureList WorldTextures;
    public MaterialList WorldMaterials;
    public TextureList PropTextures;
    public List<PropClass> PropClasses;
    public List<PropInstance> PropInstances;
    public Terrain Terrain;

    public ScenarioVariant(Scenario owner, int number)
    {
        Owner = owner;
        Number = number;
        RootDir = Path.Combine(Owner.RootDir, "V" + Number);

        Sounds = new();
        WorldTextures = new();
        WorldMaterials = new(WorldTextures);
        PropTextures = new();
        PropClasses = new();
        PropInstances = new();
        Terrain = new(WorldMaterials);
    }

    public void PeakHead()
    {

    }

    public void LoadData()
    {
        switch (Owner.Game.Version)
        {
            case >= GameVersion.C11:
            {
                var io = new ScenarioImporterCT(this);
                io.LoadAndAssign();
            }
            break;
            case >= GameVersion.MBWR:
            {
                var io = new ScenarioImporterWR(this);
                io.LoadAndAssign();
            }
            break;
        }
    }

    public void SaveData()
    {

    }
}

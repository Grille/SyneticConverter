using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SyneticLib.IO.Synetic;

namespace SyneticLib;

public partial class ScenarioVariant
{
    public readonly Scenario Owner;
    public int Number;
    public string RootDir;

    public int Width, Height;


    public GroundModel GroundModel;
    public List<Sound> Sounds;
    public TextureFolder WorldTextures;
    public MaterialList WorldMaterials;
    public MeshFolder PropMeshes;
    public List<PropClass> PropClasses;
    public List<PropInstance> PropInstances;
    public Terrain Terrain;

    public List<Light> Lights;

    public List<string> Errors;

    public InitState State { get; internal set; }

    public ScenarioVariant(Scenario owner, int number)
    {
        Owner = owner;
        Number = number;
        RootDir = Path.Combine(Owner.RootDir, "V" + Number);

        Sounds = new();
        WorldTextures = new();
        WorldMaterials = new(WorldTextures);
        PropMeshes = new();
        PropClasses = new();
        PropInstances = new();
        Terrain = new(WorldMaterials);
        Lights = new();

        Errors = new();
        State = InitState.Empty;
    }

    public void PeakHead()
    {
        WorldTextures.SeekPtxFiles(Path.Combine(RootDir, "Textures"));
    }

    public void LoadData()
    {
        switch (Owner.Game.Version)
        {
            case >= GameVersion.C11:
            {
                var io = new ScenarioImporterCT(this);
                io.Init();
            }
            break;
            case >= GameVersion.MBWR:
            {
                var io = new ScenarioImporterWR(this);
                io.Init();
            }
            break;
        }
    }

    public void SaveData()
    {

    }
}

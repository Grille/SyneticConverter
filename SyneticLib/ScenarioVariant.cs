using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SyneticLib.IO.Synetic;

namespace SyneticLib;

public partial class ScenarioVariant : Ressource
{
    public int IdNumber;
    public int Width, Height;

    public new Scenario Parent { get => (Scenario)base.Parent; set => base.Parent = value; }


    public GroundModel GroundModel;
    public Terrain Terrain;
    public RessourceDirectory<Sound> Sounds;
    public TextureDirectory WorldTextures;
    public RessourceList<TerrainMaterial> WorldMaterials;
    public MeshDirectory Objects;
    public TextureDirectory ObjectTextures;
    public RessourceList<PropClass> PropClasses;
    public RessourceList<PropInstance> PropInstances;

    public List<Light> Lights;

    public List<string> Errors;

    //public InitState State { get; internal set; }

    public ScenarioVariant(Scenario parent, int number): base(parent, PointerType.Directory)
    {
        PointerType = PointerType.Directory;

        IdNumber = number;
        SourcePath =  Parent.ChildPath($"V{IdNumber}");

        Sounds = Parent.Parent.Sounds;
        WorldTextures = new(this, ChildPath("Textures"));
        WorldMaterials = new(WorldTextures);
        ObjectTextures = new(this, ChildPath("Objects/Textures"));
        Objects = new(this, ObjectTextures, ChildPath("Objects"));
        PropClasses = new(this);
        PropInstances = new(this);
        //Terrain = new(WorldMaterials);
        Lights = new();

        Errors = new();
        //State = InitState.Empty;
    }

    public void PeakHead()
    {

    }

    protected override void OnLoad()
    {
        switch (Parent.Parent.Parent.Version)
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

    protected override void OnSave()
    {

    }

    protected override void OnSeek()
    {
        throw new NotImplementedException();
    }
}

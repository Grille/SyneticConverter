using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SyneticLib.IO.Synetic;
using System.Diagnostics;

namespace SyneticLib;

public partial class ScenarioVariant : Ressource
{
    public int IdNumber;
    public int Width, Height;

    public new Scenario Parent { get => (Scenario)base.Parent; set => base.Parent = value; }


    public GroundModel GroundModel;
    public Terrain Terrain;
    public RessourceDirectory<Sound> Sounds;
    public TextureDirectory TerrainTextures;
    public RessourceList<TerrainMaterial> TerrainMaterials;
    public ModelDirectory Objects;
    public TextureDirectory ObjectTextures;
    public RessourceList<PropClass> PropClasses;
    public RessourceList<PropInstance> PropInstances;

    public RessourceList<Light> Lights;

    public List<string> Errors;

    public ProgressInfo Progress;

    //public InitState State { get; internal set; }

    public ScenarioVariant(Scenario parent, int number): base(parent, parent.ChildPath($"V{number}"), PointerType.Directory)
    {
        IdNumber = number;

        Sounds = Parent.Parent.Sounds;

        TerrainTextures = new(this, ChildPath("Textures"));
        TerrainMaterials = new(this, ChildPath("TerrainMaterials"));
        Terrain = new(this);

        ObjectTextures = new(this, ChildPath("Objects/Textures"));
        Objects = new(this, ObjectTextures, ChildPath("Objects"));

        PropClasses = new(this, ChildPath("PropClasses"));
        PropInstances = new(this, ChildPath("PropInstances"));
        Lights = new(this, ChildPath("Lights"));

        Errors = new();
    }

    public void PeakHead()
    {

    }

    protected override void OnLoad()
    {
        Sounds.Load();
        TerrainTextures.Load();
        ObjectTextures.Load();
        //Objects.Load();

        switch (Version)
        {
            case >= GameVersion.C11:
            {
                new ScenarioImporterCT(this).Load();
            }
            break;
            case >= GameVersion.MBWR:
            {
                new ScenarioImporterWR(this).Load();
            }
            break;
        }
    }

    protected override void OnSave()
    {

    }

    protected override void OnSeek()
    {

    }
}

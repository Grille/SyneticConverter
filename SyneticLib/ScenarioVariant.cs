﻿using System;
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
    public TextureDirectory TerrainTextures;
    public RessourceList<TerrainMaterial> TerrainMaterials;
    public MeshDirectory Objects;
    public TextureDirectory ObjectTextures;
    public RessourceList<PropClass> PropClasses;
    public RessourceList<PropInstance> PropInstances;

    public RessourceList<Light> Lights;

    public List<string> Errors;

    //public InitState State { get; internal set; }

    public ScenarioVariant(Scenario parent, int number): base(parent, PointerType.Directory)
    {
        IdNumber = number;
        SourcePath =  Parent.ChildPath($"V{IdNumber}");


        Sounds = Parent.Parent.Sounds;

        TerrainTextures = new(this, ChildPath("Textures"));
        TerrainMaterials = new(this);
        Terrain = new(this);

        ObjectTextures = new(this, ChildPath("Objects/Textures"));
        Objects = new(this, ObjectTextures, ChildPath("Objects"));

        PropClasses = new(this);
        PropInstances = new(this);
        Lights = new(this);

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
        Objects.Load();

        switch (Version)
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

    }
}

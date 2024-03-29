﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using SyneticLib.Locations;

namespace SyneticLib;

public partial class Scenario : SyneticObject
{
    public int VNumber;
    public int Width, Height;

    public Model Terrain;
    public LazyRessourceDirectory<Sound> Sounds;
    public TextureDirectory TerrainTextures;
    public ModelDirectory Models;
    public TextureDirectory ModelTextures;
    public RessourceList<PropClass> PropClasses;
    public RessourceList<PropInstance> PropInstances;
    public RessourceList<Light> Lights;
    public RessourceList<ScenarioChunk> Chunks;

    public ProgressLogger Progress;

    //public InitState State { get; internal set; }

    public Scenario(int number): base($"V{number}")
    {
        VNumber = number;

        /*
        Sounds = Parent.Parent.Sounds;

        TerrainTextures = new(this, ChildPath("Textures"));
        TerrainMaterials = new(this, ChildPath("TerrainMaterials"));
        Terrain = new(this);

        ModelTextures = new(this, ChildPath("Objects/Textures"));
        Models = new(this, ModelTextures, ChildPath("Objects"));

        PropClasses = new(this, ChildPath("PropClasses"));
        PropInstances = new(this, ChildPath("PropInstances"));
        Lights = new(this, ChildPath("Lights"));
        Chunks = new(this, ChildPath("Chunks"));
        */
    }

    public void PeakHead()
    {

    }

    /*
    protected override void OnLoad()
    {
        new ScenarioImporterSynetic(this.Parent).LoadV(VNumber);
    }

    protected override void OnSave()
    {

    }

    protected override void OnSeek()
    {

    }
    */
}

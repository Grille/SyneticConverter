﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.Graphics;

namespace SyneticLib;

public class Terrain : Mesh
{
    public Terrain(ScenarioVariant parrent) : base(parrent, parrent.ChildPath("Terrain"), PointerType.Virtual)
    {
        GLBuffer = new TerrainBuffer(this);
    }

    public void CalculateChunks()
    {
        var list = new List<ScenarioChunk>();

        //return list;
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
        //throw new NotImplementedException();
    }

    public override void ExportAsObj(string path)
    {
        throw new NotImplementedException();
    }
}

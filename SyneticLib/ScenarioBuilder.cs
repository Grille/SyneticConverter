﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class ScenarioBuilder
{
    public TerrainModel? TerrainModel { get; set; }

    public List<Light> Lights { get; }

    public ScenarioBuilder()
    {
        Lights = new List<Light>();
    }

    public void BuildTerrainFromModel(Model model)
    {

    }
}

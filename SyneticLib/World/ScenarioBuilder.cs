using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.World;
public class ScenarioBuilder
{
    public SunLight? SunLight { get; set; }

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

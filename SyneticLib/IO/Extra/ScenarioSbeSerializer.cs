using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;
using SyneticLib.Locations;
using SyneticLib.World;
using SyneticLib.Files.Extra;
using SyneticLib.IO.Generic;

namespace SyneticLib.IO.Extra;

public class ScenarioSbeSerializer : DirectoryFileSerializer<Scenario>
{
    protected override Scenario OnLoad(string dirPath, string fileName)
    {
        throw new NotSupportedException();
    }

    protected override void OnSave(string dirPath, string fileName, Scenario scn)
    {
        Serializers.Terrain.Sbe.Save(dirPath, fileName, scn.Terrain);
    }
}

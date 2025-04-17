using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files.Extra;
using SyneticLib.IO.Generic;
using SyneticLib.World;

namespace SyneticLib.IO.Extra;
public class TerrainWavefrontSerializer : DirectoryFileSerializer<TerrainModel>
{
    protected override TerrainModel OnLoad(string dirPath, string fileName)
    {
        throw new NotImplementedException();
    }

    protected override void OnSave(string dirPath, string fileName, TerrainModel terrain)
    {
        Serializers.Model.Wavefront.Save(dirPath, fileName, terrain.ToModel());
    }
}


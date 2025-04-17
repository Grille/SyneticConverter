using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.IO.Generic;
using SyneticLib.World;

namespace SyneticLib.IO.Extra;
public class TerrainGltfSerializer : DirectoryFileSerializer<TerrainModel>
{
    protected override TerrainModel OnLoad(string dirPath, string fileName)
    {
        throw new NotImplementedException();
    }

    protected override void OnSave(string dirPath, string fileName, TerrainModel obj)
    {
        throw new NotImplementedException();
    }
}

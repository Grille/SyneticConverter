using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SyneticLib.Locations;

using static System.IO.Path;
using SyneticLib.IO;
using static SyneticLib.IO.Serializers;

namespace SyneticLib;
public class ModelDirectory : LazyRessourceDirectory<Model>
{
    static bool filter(string filePath) => File.Exists(filePath) && GetExtension(filePath).ToLower() == ".mox";

    static Model constructor(string filePath, TextureDirectory textures)
    {
        var model = Serializers.Model.Synetic.Load(filePath);
        model.Name = Path.GetFileNameWithoutExtension(filePath);
        return model;
    }

    public ModelDirectory(string dirPath, TextureDirectory textures) : base(dirPath, filter, (filePath) => constructor(filePath, textures))
    {
        //TextureFolder = new();
    }
}

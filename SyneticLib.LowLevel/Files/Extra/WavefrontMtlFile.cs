using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

using SyneticLib.Files.Common;

namespace SyneticLib.Files.Extra;

public class WavefrontMtlFile : TextFile
{
    public class MtlMaterial
    {
        public string Name;
        public Vector3 AmbiantColor;
        public Vector3 DiffuseColor;
        public Vector3 SpecularColor;
        public string? DiffuseMap;
    }

    public MtlMaterial[]? Materials;

    public override void Deserialize(StreamReader reader)
    {
        throw new NotImplementedException();
    }

    public override void Serialize(StreamWriter writer)
    {
        if (Materials == null)
        {
            return;
        }

        foreach (var mat in Materials)
        {
            writer.WriteLine($"newmtl {mat.Name}");
            if (mat.DiffuseMap != null)
            {
                writer.WriteLine($"map_Kd {mat.DiffuseMap}");
            }
            writer.WriteLine();
        }
    }
}

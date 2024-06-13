using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyneticLib.Files;

using SyneticLib.Locations;

namespace SyneticLib.IO;
public static partial class Imports
{
    public static Model LoadModelFromCob(string path)
    {
        var cob = new CobFile();
        cob.Load(path);

        var mesh = new Mesh(cob.Vertecis, cob.Indices);

        var segment = new MeshSegment(mesh);

        var material = new Material()
        {
            Diffuse = new OpenTK.Mathematics.Vector3(0.5f, 0.5f, 0.5f)
        };

        var model = new Model(segment, material);

        return model;
    }
}

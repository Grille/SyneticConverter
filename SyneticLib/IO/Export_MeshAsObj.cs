using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

using SyneticLib.Files.Extra;

namespace SyneticLib.IO;
public static partial class Exports
{
    //public Vector3 PositionMultiplier = Vector3.One;

    public static void SaveModelAsObj(Model model, WavefrontObjFile objFile, WavefrontMtlFile mtlFile)
    {
        /*
        var PositionMultiplier = Vector3.One;

        var mesh = model.Mesh;

        var mtlpath = Path.ChangeExtension(path, "mtl");
        {
            using var fs = new FileStream(path, FileMode.Create);
            using var sw = new StreamWriter(fs);


            sw.WriteLine($"mtllib {mtlpath}");
            sw.WriteLine($"o {mesh.Name}");

            for (var i = 0; i < mesh.Vertices.Length; i++)
            {
                var pos = mesh.Vertices[i].Position * PositionMultiplier;
                sw.WriteLine($"v {pos.X} {pos.Y} {pos.Z}");
            }

            for (var i = 0; i < mesh.Vertices.Length; i++)
            {
                var norm = mesh.Vertices[i].Normal;
                sw.WriteLine($"vn {norm.X} {norm.Y} {norm.Z}");
            }

            for (var i = 0; i < mesh.Vertices.Length; i++)
            {
                var uv = mesh.Vertices[i].UV0;
                sw.WriteLine($"vt {uv.X} {uv.Y}");
            }


            sw.WriteLine("s on");
            foreach (var reg in model.MaterialRegions)
            {
                sw.WriteLine($"usemtl {(reg.Material).Name}");
                var begin = reg.ElementOffset;
                var end = begin + reg.ElementCount;
                for (var i = begin; i < end; i++)
                {
                    ref var poly = ref mesh.Indices[i];
                    var idx0 = poly.X + 1;
                    var idx1 = poly.Y + 1;
                    var idx2 = poly.Z + 1;
                    sw.WriteLine($"f {idx0}/{idx0}/{idx0} {idx1}/{idx1}/{idx1} {idx2}/{idx2}/{idx2}");
                }
            }

        }

        /*
        {
            var rnd = new Random(1);

            using var fs = new FileStream(mtlpath, FileMode.Create);
            using var sw = new StreamWriter(fs);

            for (var i = 0; i < target.Materials.Count; i++)
            {
                var mat = target.Materials[i];

                sw.WriteLine($"newmtl {mat.Name}");
                sw.WriteLine($"Kd {rnd.NextDouble()} {rnd.NextDouble()} {rnd.NextDouble()}");
                sw.WriteLine();
            }
        }
        */

        /*
        
        foreach (var poly in target.Poligons)
        {
            sw.WriteLine($"f {poly.X + 1} {poly.Y + 1} {poly.Z + 1}");
        }
        */



    }

}

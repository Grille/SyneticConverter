﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SyneticConverter;
public class MeshExporterObj : MeshExporter
{
    public Vector3 PositionMultiplier = Vector3.One;
    public MeshExporterObj(Mesh target) : base(target)
    {

    }

    public override void Save()
    {
        var mtlpath = Path.ChangeExtension(path, "mtl");

        {
            using var fs = new FileStream(path, FileMode.Create);
            using var sw = new StreamWriter(fs);


            sw.WriteLine($"mtllib {mtlpath}");
            sw.WriteLine($"o {target.Name}");

            for (int i = 0; i < target.Vertices.Length; i++)
            {
                var pos = target.Vertices[i].Position * PositionMultiplier;
                sw.WriteLine($"v {pos.X} {pos.Y} {pos.Z}");
            }

            for (int i = 0; i < target.Vertices.Length; i++)
            {
                var uv = target.Vertices[i].UV;
                sw.WriteLine($"vt {uv.X} {uv.Y}");
            }

            sw.WriteLine("s on");
            foreach (var reg in target.PolyRegion)
            {
                sw.WriteLine($"usemtl {reg.Material.Name}");
                int begin = reg.Offset;
                int end = begin + reg.Count;
                for (int i = begin; i < end; i++)
                {
                    ref var poly = ref target.Poligons[i];
                    sw.WriteLine($"f {poly.X + 1} {poly.Y + 1} {poly.Z + 1}");
                }
            }
        }

        {
            var rnd = new Random(1);

            using var fs = new FileStream(mtlpath, FileMode.Create);
            using var sw = new StreamWriter(fs);

            for (int i = 0; i < target.Materials.Count; i++)
            {
                var mat = target.Materials[i];

                sw.WriteLine($"newmtl {mat.Name}");
                sw.WriteLine($"Kd {rnd.NextDouble()} {rnd.NextDouble()} {rnd.NextDouble()}");
                sw.WriteLine();
            } 
        }
        
        /*
        
        foreach (var poly in target.Poligons)
        {
            sw.WriteLine($"f {poly.X + 1} {poly.Y + 1} {poly.Z + 1}");
        }
        */


        
    }

}

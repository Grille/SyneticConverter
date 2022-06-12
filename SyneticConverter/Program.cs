using System;
using GGL.IO;
using SyneticConverter;

namespace SyneticConverter
{
    internal class Program
    {
        static unsafe void Main(string[] args)
        {
            //var mesh = new Mesh();
            //mesh.ImportMox(@"X:\Games\Synetic\World Racing 2\Autos\280sl\280sl.mox");
            //mesh.ExportObj("car.obj");


            var game = new GameFolder("C:/World Racing 2", TargetFormat.WR2);
            var list = game.GetAllScenarios();

            foreach (var variant in list)
            {
                Console.WriteLine(variant.Key);
            }

            var scn = list["Egypten"];

            scn.Variants[0].LoadData();

            scn.Variants[0].Terrain.Mesh.ExportObj("world.obj");

            foreach (var obj in scn.Variants[0].WorldMaterials)
            {
                Console.WriteLine(obj.Name);
            }

            //scn.Variants[0].PropClasses[0].Mesh.ExportObj("mesh.obj");




        }
    }
}

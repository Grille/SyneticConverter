using System;
using GGL.IO;
using SyneticConverter;

namespace SyneticConverter
{
    internal class Program
    {
        static unsafe void Main(string[] args)
        {
            var mesh = new Mesh(null);
            mesh.ImportMox(@"X:\Games\Synetic\World Racing 2\Autos\280sl\280sl.mox");
            mesh.ExportObj("car.obj");
            return;

            var gamewr1 = new GameFolder("C:/TDK/World Racing");
            var gamewr2 = new GameFolder("C:/World Racing 2");
            var gamect0 = new GameFolder("X:/Games/Synetic/Cobra 11 - Nitro");
            var gamect1 = new GameFolder("X:/Games/Synetic/Cobra 11 - Crash Time");
            var gamect2 = new GameFolder("X:/Games/Synetic/Cobra 11 - Burning Wheels");
            var gamect3 = new GameFolder("X:/Games/Synetic/Cobra 11 - Highway Nights");
            var gamect4 = new GameFolder("X:/Games/Synetic/Cobra 11 - Das Syndikat");
            var gamect5 = new GameFolder("X:/Games/Synetic/Cobra 11 - Undercover");

            var list = gamect5.GetAllScenarios();

            foreach (var variant in list)
            {
                Console.WriteLine(variant.Key);
            }

            var scn = list["Stadium4"];

            scn.Variants[0].LoadData();

            scn.Variants[0].Terrain.Mesh.ExportObj("world.obj");
            //scn.Variants[0].Terrain.Mesh.ExportSbi("world.synx");

            foreach (var obj in scn.Variants[0].WorldMaterials)
            {
                Console.WriteLine(obj.Name);
            }

            //scn.Variants[0].PropClasses[0].Mesh.ExportObj("mesh.obj");




        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGL.IO;

namespace SyneticLib.IO.Synetic.Files;
public class SkyFile : FileText
{
    List<SkyData> Skys;
    public override void ReadFromFile(StreamReader r)
    {
        SkyData sky = null;

        while (!r.EndOfStream)
        {
            var line = r.ReadLine();
            var split = line.Split(' ',2);
            if (split.Length != 2)
                continue;

            var key = split[0].Trim();
            var value = split[1].Trim();

            switch (key)
            {
                case "#":
                {
                    sky = new SkyData();
                    sky.Name = value;
                    Skys.Add(sky);
                    break;
                }
                case "SkyTex": { sky.SkyTex = value; break; }
                case "FogTab": { sky.FogTab = value; break; }
                case "FogCol": { sky.FogCol = value; break; }
                case "SunCol": { sky.SunCol = value; break; }
                case "AmbCol": { sky.AmbCol = value; break; }
                case "WlkAmb": { sky.WlkAmb = value; break; }
                case "WlkSun": { sky.WlkSun = value; break; }
                case "CarShd": { sky.CarShd = value; break; }
            }
        }
    }

    public override void WriteToFile(StreamWriter w)
    {
        foreach (var sky in Skys)
        {
            w.WriteLine($"# {sky.Name}");

            w.WriteLine($"SkyTex {sky.SkyTex}");
            w.WriteLine($"FogTab {sky.FogTab}");
            w.WriteLine($"FogCol {sky.FogCol}");
            w.WriteLine($"SunCol {sky.SunCol}");
            w.WriteLine($"AmbCol {sky.AmbCol}");
            w.WriteLine($"WlkAmb {sky.WlkAmb}");
            w.WriteLine($"WlkSun {sky.WlkSun}");
            w.WriteLine($"CarShd {sky.CarShd}");
        }
    }

    class SkyData
    {
        public string Name;
        public string SkyTex;
        public string FogTab;
        public string FogCol;
        public string SunCol;
        public string AmbCol;
        public string WlkAmb;
        public string WlkSun;
        public string CarShd;
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grille.IO;
using SyneticLib.Files.Common;

namespace SyneticLib.Files;
public class SkyFile : SyneticIniFile
{
    List<SkyData> Skies { get; }

    public SkyFile()
    {
        Skies = new List<SkyData>();
    }

    protected override void OnRead()
    {
        Skies.Clear();

        foreach (var section in Sections)
        {
            var sky = new SkyData(section.Name);
            Skies.Add(sky);

            foreach (var pair in section)
            {
                switch (pair.Key)
                {
                    case "SkyTex": { sky.SkyTex = pair.Value; break; }
                    case "FogTab": { sky.FogTab = pair.Value; break; }
                    case "FogCol": { sky.FogCol = pair.Value; break; }
                    case "SunCol": { sky.SunCol = pair.Value; break; }
                    case "AmbCol": { sky.AmbCol = pair.Value; break; }
                    case "WlkAmb": { sky.WlkAmb = pair.Value; break; }
                    case "WlkSun": { sky.WlkSun = pair.Value; break; }
                    case "CarShd": { sky.CarShd = pair.Value; break; }
                }
            }
        }
    }

    protected override void OnWrite()
    {
        foreach (var sky in Skies)
        {
            var sec = new Section(sky.Name);
            Sections.Add(sec);

            void Add(string key, string? value)
            {
                if (value != null)
                {
                    sec.Add(key, value);
                }
            }

            Add("SkyTex", sky.SkyTex);
            Add("FogTab", sky.FogTab);
            Add("FogCol", sky.FogCol);
            Add("SunCol", sky.SunCol);
            Add("AmbCol", sky.AmbCol);
            Add("WlkAmb", sky.WlkAmb);
            Add("WlkSun", sky.WlkSun);
            Add("CarShd", sky.CarShd);
        }
    }

    class SkyData
    {
        public string Name;

        public string? SkyTex;
        public string? FogTab;
        public string? FogCol;
        public string? SunCol;
        public string? AmbCol;
        public string? WlkAmb;
        public string? WlkSun;
        public string? CarShd;

        public SkyData(string name)
        {
            Name = name;
        }
    }
}

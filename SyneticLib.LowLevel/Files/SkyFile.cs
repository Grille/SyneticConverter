using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGL.IO;

namespace SyneticLib.LowLevel.Files;
public class SkyFile : SyneticIniFile
{
    List<SkyData> Skies = new List<SkyData>();
    protected override void OnRead()
    {
        Skies.Clear();

        foreach (var section in Sections)
        {
            var sky = new SkyData() { Name = section.Name };
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
            var sec = new Section() { Name = sky.Name };
            Sections.Add(sec);

            sec.Add("SkyTex", sky.SkyTex);
            sec.Add("FogTab", sky.FogTab);
            sec.Add("FogCol", sky.FogCol);
            sec.Add("SunCol", sky.SunCol);
            sec.Add("AmbCol", sky.AmbCol);
            sec.Add("WlkAmb", sky.WlkAmb);
            sec.Add("WlkSun", sky.WlkSun);
            sec.Add("CarShd", sky.CarShd);
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

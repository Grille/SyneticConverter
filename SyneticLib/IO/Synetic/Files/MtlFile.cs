using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace SyneticLib.IO.Synetic.Files;
public class MtlFile : FileText
{
    public SHead Head;
    public List<SMaterial> Sections;

    public MtlFile()
    {
        Head = new();
        Sections = new();
    }

    public override void ReadFromFile(StreamReader r)
    {
        MtlSection usedSection = Head;
        while (!r.EndOfStream)
        {
            var line = r.ReadLine().Trim();


            var split = line.Split(' ', 2);
            if (split.Length != 2)
                continue;

            var key = split[0];
            var value = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (key == "#")
            {
                var section = new SMaterial();
                usedSection = section;
                Sections.Add(section);
            }

            usedSection.Add(key, value);
        }
    }

    public override void WriteToFile(StreamWriter w)
    {
        foreach (var pair in Head) { 
            w.WriteLine($"{pair.Key} {string.Join(' ',pair.Value)}");
        }

        foreach (var section in Sections)
        {
            foreach (var pair in section)
            {
                w.WriteLine($"{pair.Key} {string.Join(' ', pair.Value)}");
            }
        }
    }

    public class MtlSection : Dictionary<string, string[]>
    {

    }

    public class SHead : MtlSection
    {
        //public string[] ColSetInf ;
    }

    public class SMaterial : MtlSection
    {
        /*

        public int ID => (int)GetHexArray("#")[0];
        public int[] MatClass => GetIntArray("MatClass");
        public uint[] Diffuse => GetHexArray("Diffuse");
        public uint[] Ambient => GetHexArray("Ambient");
        public uint[] Specular => GetHexArray("Specular");
        public uint[] Reflect => GetHexArray("Reflect");
        public uint[] Specular2 => GetHexArray("Specular2");
        public uint[] XDiffuse => GetHexArray("XDiffuse");
        public uint[] XSpecular => GetHexArray("XSpecular");

        public string[] Transparency => this["MatClass"].Split(' ');
        public string[] Tex1Name => GetString("Tex1Name");
        public string[] Tex2Name => GetString("Tex2Name");
        public string[] Tex3Name => GetString("Tex3Name");
        public uint[] TexFlags => GetHexArray("TexFlags");
        public float[] TexOffset => GetFloatArray("TexOffset");
        public float[] TexScale => GetFloatArray("TexScale");
        public float TexAngle => GetFloatArray("TexAngle")[0];
        */
    }
}

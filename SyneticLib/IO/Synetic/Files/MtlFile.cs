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
                
            if (line.Length == 0)
                continue;

            if (line[0] == '#')
            {
                var section = new SMaterial();
                usedSection = section;
                Sections.Add(section);
            }

            var split = line.Split(' ', 2);
            var key = split[0];
            var value = split[1];
            usedSection.Add(key, value);
        }
    }

    public override void WriteToFile(StreamWriter w)
    {
        throw new NotImplementedException();
    }

    public class MtlSection : Dictionary<string, string>
    {
        public uint[] GetHexArray(string key)
        {
            var values = this[key].Split(' ');
            var res = new uint[values.Length];
            for (var i = 0; i < values.Length; i++)
            {
                res[i] = uint.Parse(values[i].Split('x')[1], NumberStyles.HexNumber);
            }
            return res;
        }
        public int[] GetIntArray(string key)
        {
            var values = this[key].Split(' ');
            var res = new int[values.Length];
            for (var i = 0; i < values.Length; i++)
            {
                res[i] = int.Parse(values[i]);
            }
            return res;
        }

        public float[] GetFloatArray(string key)
        {
            var values = this[key].Split(' ');
            var res = new float[values.Length];
            for (var i = 0; i < values.Length; i++)
            {
                res[i] = float.Parse(values[i]);
            }
            return res;
        }

        public string[] GetStringArray(string key)
        {
            var values = this[key].Split("\" ", StringSplitOptions.RemoveEmptyEntries);
            var res = new string[values.Length];
            for (var i = 0; i < values.Length; i++)
            {
                res[i] = values[i].Trim('"');
            }
            return res;
        }

        public string GetString(string key)
        {
            return this[key].ToString().Trim('"');
        }




    }

    public class SHead : MtlSection
    {
        public string[] ColSetInf ;
    }

    public class SMaterial : MtlSection
    {
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
        public string Tex1Name => GetString("Tex1Name");
        public string Tex2Name => GetString("Tex2Name");
        public string Tex3Name => GetString("Tex3Name");
        public uint[] TexFlags => GetHexArray("TexFlags");
        public float[] TexOffset => GetFloatArray("TexOffset");
        public float[] TexScale => GetFloatArray("TexScale");
        public float TexAngle => GetFloatArray("TexAngle")[0];
    }
}

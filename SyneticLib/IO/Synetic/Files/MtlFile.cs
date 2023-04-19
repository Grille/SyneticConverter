using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace SyneticLib.IO.Synetic.Files;
public class MtlFile : SyneticTextFile
{
    public string[] ColSetInf;
    public List<SMaterial> Materials;

    public MtlFile()
    {
        Materials = new();
    }

    protected override void OnRead()
    {
        if (Head.TryGetValue("ColSetInf", out string value))
        {
            ColSetInf = Head["ColSetInf"].Split(' ');
        }
        else
        {
            ColSetInf = new[] { "\"Default\"" };
        }

        Materials.Clear();

        foreach (var section in Sections)
        {
            var material = new SMaterial() { ID = section.Name };
            Materials.Add(material);

            foreach (var pair in section)
            {
                switch (pair.Key)
                {
                    case "Tex1Name": { material.Tex1Name = pair.Value.Trim('"'); break; }
                }
            }
        }
    }

    protected override void OnWrite()
    {

    }
    /*
    uint[] ReadHexArray(Section section, string key, int size)
    {
        var split = section[key].Split(' ');


        for (int i = 0;i< split.Length;i++)
        {

        }
    }
    */

    public class MtlSection : Dictionary<string, string[]>
    {

    }

    public class SHead : MtlSection
    {
        public string[] ColSetInf;
    }

    public class SMaterial : MtlSection
    {
        public string ID;
        public int[] MatClass;
        public uint[] Diffuse;
        public uint[] Ambient;
        public uint[] Specular;
        public uint[] Reflect;
        public uint[] Specular2;
        public uint[] XDiffuse;
        public uint[] XSpecular;

        public string[] Transparency;
        public string Tex1Name;
        public string Tex2Name;
        public string Tex3Name;
        public uint[] TexFlags;
        public float[] TexOffset;
        public float[] TexScale;
        public float TexAngle;

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

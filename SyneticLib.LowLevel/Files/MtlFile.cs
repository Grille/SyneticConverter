using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using OpenTK.Mathematics;
using SyneticLib.Files.Common;

namespace SyneticLib.Files;
public class MtlFile : SyneticIniFile
{
    public string[] ColSetInf;
    public List<SMaterial> Materials;

    public MtlFile()
    {
        ColSetInf = Array.Empty<string>();
        Materials = new();
    }

    protected override void OnRead()
    {

        if (Head.TryGetValue("ColSetInf", out var value))
        {
            ColSetInf = Head["ColSetInf"].Split(' ');
        }
        else
        {
            ColSetInf = new[] { "_default_" };
        }

        Materials.Clear();

        foreach (var section in Sections)
        {
            var material = new SMaterial(section.Name, ColSetInf.Length);

            foreach (var pair in section)
            {
                switch (pair.Key)
                {
                    case "Diffuse": ParseHexArray(material.Diffuse, pair.Value); break;
                    case "Tex1Name": material.Tex1Name = ParseString(pair.Value); break;
                    case "Tex2Name": material.Tex2Name = ParseString(pair.Value); break;
                    case "Tex3Name": material.Tex3Name = ParseString(pair.Value); break;
                }
            }

            Materials.Add(material);
        }

    }

    protected override void OnWrite()
    {

    }

    static void ParseHexArray(BgraColor[] dst, string src)
    {
        var colors = ParseHexArray(src);
    }

    static uint[] ParseHexArray(string value)
    {
        var split = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        uint[] result = new uint[split.Length];

        for (int i = 0; i < split.Length; i++)
        {
            string nrstring = split[i].Substring(2, split[i].Length - 2);
            result[i] = uint.Parse(nrstring, NumberStyles.HexNumber);
        }

        return result;
    }

    static string ParseString(string value)
    {
        return value.Trim('"', ' ');
    }

    //static int Parse

    static float ParseSingle(string value)
    {
        return 0;
    }

    static Vector2 ParseVec2(string value)
    {
        var split = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        float x = float.Parse(split[0]);
        float y = float.Parse(split[1]);

        return new Vector2(x, y);
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


    public class SMaterial
    {
        public readonly string Name;
        public readonly int Length;

        public readonly BgraColor[] Diffuse;
        public readonly BgraColor[] Ambient;
        public readonly BgraColor[] Specular;
        public readonly BgraColor[] Reflect;
        public readonly BgraColor[] Specular2;
        public readonly BgraColor[] XDiffuse;
        public readonly BgraColor[] XSpecular;

        public MMatClass MatClass;
        public float Transparency;
        public string Tex1Name;
        public string Tex2Name;
        public string Tex3Name;
        public MTexFlags TexFlags;
        public Vector2 TexOffset;
        public Vector2 TexScale;
        public float TexAngle;

        public struct MMatClass
        {
            public byte A, B, C, D;
        }

        public struct MTexFlags
        {
            public byte A, B, C, D;
        }

        public SMaterial(string name, int length)
        {
            Name = name;
            Length = length;

            Tex1Name = string.Empty;
            Tex2Name = string.Empty;
            Tex3Name = string.Empty;

            Diffuse = new BgraColor[length];
            Ambient = new BgraColor[length];
            Specular = new BgraColor[length];
            Reflect = new BgraColor[length];
            Specular2 = new BgraColor[length];
            XDiffuse = new BgraColor[length];
            XSpecular = new BgraColor[length];
        }
    }
}

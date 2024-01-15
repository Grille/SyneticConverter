using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using OpenTK.Mathematics;

namespace SyneticLib.LowLevel.Files;
public class MtlFile : SyneticIniFile
{
    public string[] ColSetInf;
    public List<SMaterial> Materials;

    public MtlFile()
    {
        Materials = new();
    }

    protected override void OnRead()
    {
        /*
        if (Head.TryGetValue("ColSetInf", out string value))
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
            var material = new SMaterial();
            material.Name = section.Name;

            foreach (var pair in section)
            {
                switch (pair.Key)
                {
                    case "Diffuse": material.Diffuse = ParseHexArray(pair.Value); break;
                    case "Tex1Name": material.Tex1Name = ParseString(pair.Value); break;
                    case "Tex2Name": material.Tex2Name = ParseString(pair.Value); break;
                    case "Tex3Name": material.Tex3Name = ParseString(pair.Value); break;
                }
            }

            Materials.Add(material);
        }
        */
    }

    protected override void OnWrite()
    {

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
        public string Name;
        
        public MMatClass MatClass;
        public BgraColor[] Diffuse;
        public BgraColor[] Ambient;
        public BgraColor[] Specular;
        public BgraColor[] Reflect;
        public BgraColor[] Specular2;
        public BgraColor[] XDiffuse;
        public BgraColor[] XSpecular;

        public float[] Transparency;
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
    }
}

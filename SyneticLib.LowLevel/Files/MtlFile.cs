using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using OpenTK.Mathematics;
using SyneticLib.Files.Common;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace SyneticLib.Files;
public class MtlFile : SyneticCfgFile<MtlFile.MtlMaterial>
{
    public static class WR1ColorIndex
    {
        public const int Schwarz = 0;
        public const int Rot = 1;
        public const int DunkelRot = 2;
        public const int Gelb = 3;
        public const int Gold = 4;
        public const int Weiss = 5;
        public const int Anthrazit = 6;
        public const int DunkelGruen = 7;
        public const int Blau = 8;
        public const int HellBlau = 9;
        public const int QuarzBlau = 10;
        public const int Gruen = 11;
        public const int Silber = 12;
        public const int Heliodor = 13;
        public const int Tuerkis = 14;
    }

    public static class WR2ColorIndex
    {
        public const int Schwarz = 0;
        public const int Rot1 = 1;
        public const int Rot2 = 2;
        public const int DunkelRot = 3;
        public const int Gelb1 = 4;
        public const int Gelb2 = 5;
        public const int Weiss = 6;
        public const int Anthrazit = 7;
        public const int DunkelGruen1 = 8;
        public const int DunkelGruen2 = 9;
        public const int Blau1 = 10;
        public const int QuarzBlau = 11;
        public const int Blau2 = 12;
        public const int Silber1 = 13;
        public const int Silber2 = 14;
    }

    public static class PropertySerializer
    {
        public static int DeserializeHexNumber(string value)
        {
            return int.Parse(value.Substring(2), NumberStyles.HexNumber);
        }

        public static string SerializeHexNumber(int value)
        {
            return $"{value & 0x00ffffff:x6}";
        }

        public static float DeserializePercentage(string value)
        {
            return int.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture) / 100f;
        }

        public static string SerializePercentage(float value)
        {
            return $"{(int)(value * 100):3}";
        }

        public static float DeserializeSingle(string value)
        {
            return float.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        public static string SerializeSingle(float value)
        {
            return $"{value:0.000000}";
        }

        public static string DeserializeString(string value)
        {
            return value.Substring(1, value.Length-2);
        }

        public static string SerializeString(string value)
        {
            return $"\"{value}\"";
        }
    }

    public class HexArrayProperty : SyneticCfgFileArrayProperty<int>
    {
        public HexArrayProperty(Dictionary<string, string> dict, string key) : base(dict, key) { }

        protected override int DeserializeItem(string value) => PropertySerializer.DeserializeHexNumber(value);

        protected override string SerializeItem(int value) => PropertySerializer.SerializeHexNumber(value);
    }

    public class SingleProperty : SyneticCfgFileProperty<float>
    {
        public SingleProperty(Dictionary<string, string> dict, string key) : base(dict, key) { }

        protected override float Deserialize(string value) => PropertySerializer.DeserializeSingle(value);

        protected override string Serialize(float value) => PropertySerializer.SerializeSingle(value);
    }

    public class PercentageProperty : SyneticCfgFileProperty<float>
    {
        public PercentageProperty(Dictionary<string, string> dict, string key) : base(dict, key) { }

        protected override float Deserialize(string value) => PropertySerializer.DeserializePercentage(value);

        protected override string Serialize(float value) => PropertySerializer.SerializePercentage(value);
    }

    public class TextureProperty : SyneticCfgFileProperty<string>
    {
        public TextureProperty(Dictionary<string, string> dict, string key) : base(dict, key) { }

        protected override string Deserialize(string value) => PropertySerializer.DeserializeString(value);

        protected override string Serialize(string value) => PropertySerializer.SerializeString(value);
    }

    public class MtlMaterial : Dictionary<string, string>
    {
        public PercentageProperty Alpha { get; }
        public SingleProperty Transparency { get; }
        public HexArrayProperty Ambient { get; }
        public HexArrayProperty Diffuse { get; }
        public HexArrayProperty Reflect { get; }
        public HexArrayProperty Reflect2 { get; }
        public HexArrayProperty Specular { get; }
        public HexArrayProperty Specular2 { get; }
        public HexArrayProperty XDiffuse { get; }
        public HexArrayProperty XSpecular { get; }
        public TextureProperty Tex1Name { get; }

        public MtlMaterial()
        {
            Alpha = new(this, "Alpha");
            Transparency = new(this, "Transparency");

            Ambient = new(this, "Ambient");
            Diffuse = new(this, "Diffuse");
            Reflect = new(this, "Reflect");
            Reflect2 = new(this, "Reflect2");
            Specular = new(this, "Specular");
            Specular2 = new(this, "Specular2");
            XDiffuse = new(this, "XDiffuse");
            XSpecular = new(this, "XSpecular");
            Tex1Name = new(this, "Tex1Name");
        }
    }

    public SyneticCfgFileProperty ColSetInf { get; }

    public MtlFile()
    {
        ColSetInf = new(Head, "ColSetInf");
    }
}

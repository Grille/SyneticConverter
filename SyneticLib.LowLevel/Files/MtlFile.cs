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
public class MtlFile : SyneticIniFile<MtlFile.MtlMaterial>
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

    public class MtlColor
    {
        public int Ambient;
        public int Diffuse;
        public int Reflect;
        public int Specular;
        public int Specular2;
        public int XDiffuse;
        public int XSpecular;
    }

    public class MtlMaterial : Dictionary<string, string>
    {
        public SyneticIniFileProperty Ambient { get; }
        public SyneticIniFileProperty Diffuse { get; }
        public SyneticIniFileProperty Reflect { get; }
        public SyneticIniFileProperty Specular { get; }
        public SyneticIniFileProperty Specular2 { get; }
        public SyneticIniFileProperty XDiffuse { get; }
        public SyneticIniFileProperty XSpecular { get; }

        public MtlColor[] Colors
        {
            get
            {
                var ambient = Ambient.Hex24Array;
                var diffuse = Diffuse.Hex24Array;
                var reflect = Reflect.Hex24Array;
                var specular = Specular.Hex24Array;
                var specular2 = Specular2.Hex24Array;
                var xDiffuse = XDiffuse.Hex24Array;
                var xSpecular = XSpecular.Hex24Array;

                var colors = new MtlColor[ambient.Length];

                for (int i = 0; i < ambient.Length; i++)
                {
                    colors[i] = new MtlColor()
                    {
                        Ambient = ambient[i],
                        Diffuse = diffuse[i],
                        Reflect = reflect[i],
                        Specular = specular[i],
                        Specular2 = specular2[i],
                        XDiffuse = xDiffuse[i],
                        XSpecular = xSpecular[i]
                    };
                }

                return colors;
            }
            set
            {
                var ambient = new int[value.Length];
                var diffuse = new int[value.Length];
                var reflect = new int[value.Length];
                var specular = new int[value.Length];
                var specular2 = new int[value.Length];
                var xDiffuse = new int[value.Length];
                var xSpecular = new int[value.Length];

                for (int i = 0; i < value.Length; i++)
                {
                    ambient[i] = value[i].Ambient;
                    diffuse[i] = value[i].Diffuse;
                    reflect[i] = value[i].Reflect;
                    specular[i] = value[i].Specular;
                    specular2[i] = value[i].Specular2;
                    xDiffuse[i] = value[i].XDiffuse;
                    xSpecular[i] = value[i].XSpecular;
                }

                Ambient.Hex24Array = ambient;
                Diffuse.Hex24Array = diffuse;
                Specular.Hex24Array = reflect;
                Specular.Hex24Array = specular;
                Specular2.Hex24Array = specular2;
                XDiffuse.Hex24Array = xDiffuse;
                XSpecular.Hex24Array = xSpecular;
            }
        }

        public MtlMaterial()
        {
            Ambient = new(this, "Ambient");
            Diffuse = new(this, "Diffuse");
            Reflect = new(this, "Reflect");
            Specular = new(this, "Specular");
            Specular2 = new(this, "Specular2");
            XDiffuse = new(this, "XDiffuse");
            XSpecular = new(this, "XSpecular");
        }
    }

    public SyneticIniFileProperty ColSetInf { get; }

    public MtlFile()
    {
        ColSetInf = new(Head, "ColSetInf");
    }
}

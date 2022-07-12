using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GGL.IO;
using System.Runtime.InteropServices;

namespace SyneticLib.IO.Synetic.Files;
public class MoxFile : SyneticBinFile
{
    public MHead Head;

    public MVertex[] Vertecis;
    public MPoly[] Indices;
    public MTex[] Textures;

    public unsafe override void ReadFromView(BinaryViewReader br)
    {
        Head = br.Read<MHead>();

        if (Head.Magic != "!XOM")
            throw new InvalidOperationException();

        var ver = (MoxVerion)Head.Version;

        Console.WriteLine($"{br.Position} / {br.Length} -> {br.Position:X}");

        Vertecis = br.ReadArray<MVertex>(Head.VertCount);

        Console.WriteLine($"{br.Position} / {br.Length} -> {br.Position:X}");

        Indices = br.ReadArray<MPoly>(Head.PolyCount);

        Console.WriteLine($"{br.Position} / {br.Length} -> {br.Position:X}");

        Textures = br.ReadArray<MTex>(Head.TextureCount);

        Console.WriteLine($"{br.Position} / {br.Length} -> {br.Position:X}");

        var size = sizeof(MVertex);




    }

    public override void WriteToView(BinaryViewWriter bw)
    {
        throw new NotImplementedException();
    }

    public struct MHead
    {
        public String4 Magic;
        public int Version;
        public int VertCount, PolyCount, TextureCount, MatCount, PartCount, LightCount;
    }

    [StructLayout(LayoutKind.Sequential, Size = 40)]
    public struct MVertex
    {
        public Vector3 Position;
        public byte a0, a1, a2, a3;
        public float b;
        public Vector3 UV;
        public float c, d;
    }

    public struct MPoly
    {
        public ushort X, Y, Z;
    }

    public struct MTex
    {
        int a, b, c, d, r, f;
    }
}

public enum MoxVerion
{
    MBWR = 65536,
    SimpleWR2 = 33554432,
    ComplexWR2 = 33685504,
}



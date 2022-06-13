﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GGL.IO;

namespace SyneticConverter;
public class VtxFile : SyneticFile
{
    public int[] VtxQty;
    public MVertex[] Vertices;

    public unsafe override void Read(BinaryViewReader br)
    {
        VtxQty = br.ReadArray<int>(64);

        int length = 0;
        for (int i = 0; i < VtxQty.Length; i++)
            length += VtxQty[i];

        long finalpos = br.Position + length * sizeof(MVertex);
        if (br.Length != finalpos)
            throw new InvalidOperationException($"{br.Length} != {finalpos}");

        Vertices = br.ReadArray<MVertex>(length);

    }

    public override void Write(BinaryViewWriter bw)
    {
        bw.WriteArray(VtxQty, LengthPrefix.None);
        bw.WriteArray(Vertices, LengthPrefix.None);
    }

    public struct MVertex
    {
        public Vector3 Position;
        public BgraColor Normal;
        public Vector2 UV;
        public BlendColor Blend;
        public BgraColor Color;
    }
}



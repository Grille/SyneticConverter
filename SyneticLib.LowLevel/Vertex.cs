﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SyneticLib.LowLevel;

/// <summary>General vertex type, can hold any info needed for all model types.</summary>
[StructLayout(LayoutKind.Explicit, Size = Layout.Size)]
public struct Vertex
{
    public static class Layout
    {
        private const int size1 = sizeof(float) * 1;
        private const int size2 = sizeof(float) * 2;
        private const int size3 = sizeof(float) * 3;

        public const int Position = 0;
        public const int Normal = Position + size3;
        public const int UV0 = Normal + size3;
        public const int UV1 = UV0 + size2;
        public const int Blending = UV1 + size2;
        public const int LightColor = Blending + size3;
        public const int Shadow = LightColor + size3;
        public const int Size = Shadow + size1;
    }


    [FieldOffset(Layout.Position)]
    public Vector3 Position;

    [FieldOffset(Layout.Normal)]
    public Vector3 Normal;

    [FieldOffset(Layout.UV0)]
    public Vector2 UV0;

    [FieldOffset(Layout.UV1)]
    public Vector2 UV1;

    [FieldOffset(Layout.Blending)]
    public Vector3 Blending;

    /// <summary>Baked-lighting values for terrains.</summary>
    [FieldOffset(Layout.LightColor)]
    public Vector3 LightColor;

    [FieldOffset(Layout.Shadow)]
    public float Shadow;

    public Vector3 InvPosition
    {
        get => new Vector3(Position.X, Position.Z, Position.Y);
        set => Position = new Vector3(value.X, value.Z, value.Y);
    }

    public Vector3 InvNormal
    {
        get => new Vector3(Normal.Z, Normal.Y, Normal.X);
        set => Normal = new Vector3(value.Z, value.Y, value.X);
    }

    public BgraColor RGBAInvNormal
    {
        get => BgraColor.FromArgb(0, (byte)(Normal.X * 255), (byte)(Normal.Y * 255), (byte)(Normal.Z * 255));
        set => Normal = new Vector3(value.R / 255f, value.G / 255f, value.B / 255f);
    }

    public BlendColor RGBABlend
    {
        get => new BlendColor()
        {
            Vec3Blend = Blending,
            FloatShadow = Shadow
        };
        set
        {
            Blending = value.Vec3Blend;
            Shadow = value.FloatShadow;
        }
    }

    public Vertex()
    {

    }

    public Vertex(Vector3 position, Vector2 uV0)
    {
        Position = position;
        UV0 = uV0;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace SyneticLib;

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
        public const int Unknown0 = Shadow + size1;
        public const int Size = Unknown0 + size1;
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

    [FieldOffset(Layout.Unknown0)]
    public float Unknown0;

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

    public Vertex() { }

    public Vertex(Vector3 position)
    {
        Position = position;
    }

    public Vertex(Vector3 position, Vector2 uV0)
    {
        Position = position;
        UV0 = uV0;
    }

    public static implicit operator Vertex(Vector3 position) => new Vertex(position);

    public static bool operator ==(in Vertex left, in Vertex right)
    {
        return left.Equals(ref Unsafe.AsRef(right));
    }

    public static bool operator !=(in Vertex left, in Vertex right)
    {
        return !(left == right);
    }

    public bool Equals(ref Vertex other)
    {
        return Position == other.Position && Normal == other.Normal && UV0 == other.UV0 && UV1 == other.UV1 && LightColor == other.LightColor && Shadow == other.Shadow;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is not Vertex vertex)
            return false;
        return Equals(ref vertex);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

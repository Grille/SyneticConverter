using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SyneticLib.LowLevel;
public struct Vertex
{
    public Vector3 Position;
    public Vector3 Normal;
    public Vector2 UV0;
    public Vector2 UV1;
    public Vector3 Blending;
    public BgraColor LightColor;
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

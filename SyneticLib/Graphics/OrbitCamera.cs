using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace SyneticLib.Graphics;
public class OrbitCamera : Camera
{
    public Vector3 Focus;
    public Vector3 Position = new Vector3(0,15,0);

    public float ScrollFactor = 1.2f;
    public float MoveFactor = 0.01f;

    public float MaxDistance = 100000;
    public float MinDistance = 10;

    public float AngleX = 0, AngleY = 0, Distance = 100;

    protected override void OnScroll(int delta)
    {
        if (delta > 0)
            Distance /= ScrollFactor;
        else
            Distance *= ScrollFactor;

        if (Distance > MaxDistance)
            Distance = MaxDistance;

        if (Distance < MinDistance)
            Distance = MinDistance;
    }

    protected override void OnMouseMove(Vector2 diff)
    {
        RotateAroundCenterHorizontal(diff.X);
        RotateAroundCenterVertical(diff.Y);
    }

    public void RotateAroundCenterHorizontal(float amount)
    {
        AngleX -= amount * MoveFactor;
    }

    public void RotateAroundCenterVertical(float amount)
    {
        AngleY += amount * MoveFactor;

        if (AngleY > MathF.PI * 0.49f)
            AngleY = MathF.PI * 0.49f;

        if (AngleY < -MathF.PI * 0.49f)
            AngleY = -MathF.PI * 0.49f;
    }

    public override void CreateView()
    {
        Console.WriteLine(AngleY);

        Position = Focus + new Vector3(0, Distance, 0);

        var cosa = MathF.Cos(AngleX);
        var sina = MathF.Sin(AngleX);

        var cosb = MathF.Cos(0);
        var sinb = MathF.Sin(0);

        var cosc = MathF.Cos(AngleY);
        var sinc = MathF.Sin(AngleY);

        var Axx = cosa * cosb;
        var Axy = cosa * sinb * sinc - sina * cosc;
        var Axz = cosa * sinb * cosc + sina * sinc;

        var Ayx = sina * cosb;
        var Ayy = sina * sinb * sinc + cosa * cosc;
        var Ayz = sina * sinb * cosc - cosa * sinc;

        var Azx = -sinb;
        var Azy = cosb * sinc;
        var Azz = cosb * cosc;

        Position.Deconstruct(out float x, out float y, out float z);
        Position.X = Axx * x + Axy * y + Axz * z;
        Position.Y = Ayx * x + Ayy * y + Ayz * z;
        Position.Z = Azx * x + Azy * y + Azz * z;

        ViewMatrix = Matrix4.LookAt(Position, Focus, Up);
    }
}

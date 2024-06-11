using OpenTK.Mathematics;
using SyneticLib.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SyneticLib.Files.QadFile;

namespace SyneticLib.Math3D;

public class FreeCamera : Camera
{
    public float ScrollFactor = 1.2f;
    public float MoveFactor = 0.01f;

    public float AngleX = MathF.PI / 8, AngleY = MathF.PI / 8, Distance = 100;

    protected override void OnScroll(int delta)
    {

    }

    protected override void OnMouseMove(Vector2 diff)
    {
        // pitch
        Rotation.X += diff.Y / 100f;

        // yaw
        Rotation.Y -= diff.X / 100f;

        const float maxPitch = MathF.PI * 0.49f;
        Rotation.X = Math.Clamp(Rotation.X, -maxPitch, maxPitch);
    }

    public override void Update(TimeSpan delta)
    {
        float pitch = Rotation.X;
        float yaw = Rotation.Y;

        float x = MathF.Cos(pitch) * MathF.Sin(yaw);
        float y = -MathF.Sin(pitch);
        float z = -MathF.Cos(pitch) * MathF.Cos(yaw);

        var forward = new Vector3(x, y, z);

        var right = Vector3.Cross(forward, Vector3.UnitY);

        forward.Normalize();
        right.Normalize();

        float speed = 1 * (float)delta.TotalMilliseconds;

        var movment = Vector3.Zero;

        if (Inputs.MoveUp)
            movment += forward * speed;
        if (Inputs.MoveDown)
            movment += forward * -speed;
        if (Inputs.MoveLeft)
            movment += right * speed;
        if (Inputs.MoveRight)
            movment += right * -speed;

        Position += movment;
    }

    public override void CreateView()
    {
        var rotation = Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationX(Rotation.X) * Matrix4.CreateRotationZ(Rotation.Z);
        var translation = Matrix4.CreateTranslation(-Position);
        var scale = Matrix4.CreateScale(-1, 1, 1);

        ViewMatrix = translation * rotation * scale;
    }

    public override Vector3 ViewportPosToVector(Vector2 position)
    {
        throw new NotImplementedException();
    }
}

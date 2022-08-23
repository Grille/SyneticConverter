using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Mathematics;

namespace SyneticLib.Graphics;
public class Camera
{
    public Matrix4 ProjectionMatrix = Matrix4.Identity;
    public Matrix4 ViewMatrix = Matrix4.Identity;

    public Vector3 Position = Vector3.Zero;
    public Vector3 Center = Vector3.Zero;
    public Vector3 Up = new Vector3(0,1,0);

    Vector2 _screenSize = Vector2.One;

    public Vector2 ScreenSize
    {
        get => _screenSize; set
        {
            AspectRatio = _screenSize.X / _screenSize.Y;
            _screenSize = value;
        }
    }

    public float AspectRatio { get; private set; }

    public Camera()
    {

    }

    public void Scroll(int delta, float factor)
    {
        float distance = MathF.Abs((Center - Position).Length);

        float newDist = distance;

        if (delta > 0)
            newDist *= factor;
        else
            newDist /= factor;

        float diff = newDist - distance;

        Move(diff);
    }

    public void RotateAroundCenterHorizontal(float amount)
    {

    }

    public void RotateAroundCenterVertical(float amount)
    {

    }


    public void Move(float amount)
    {
        var vector = (Center - Position).Normalized() * amount;
        Position += vector;
    }

    public void CreatePerspective()
    {
        ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, AspectRatio, 1.0f, 16_000_0.0f);
    }

    public void LookAt(Vector3 target)
    {
        Center = target;
        CreateView();
    }

    public void CreateView()
    {
        ViewMatrix = Matrix4.LookAt(Position, Center, Up);
    }
}

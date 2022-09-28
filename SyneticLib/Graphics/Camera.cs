using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Mathematics;

namespace SyneticLib.Graphics;
public abstract class Camera
{
    public Matrix4 ProjectionMatrix = Matrix4.Identity;
    public Matrix4 ViewMatrix = Matrix4.Identity;
    public Vector3 Up = new Vector3(0,0,1);

    Vector2 _screenSize = Vector2.One;

    public Vector2 ScreenSize
    {
        get => _screenSize; set
        {
            AspectRatio = _screenSize.X / _screenSize.Y;
            _screenSize = value;
        }
    }

    Vector2 LastMouseLocation;

    public float AspectRatio { get; private set; } = 1;

    public void Scroll(int delta)
    {
        OnScroll(delta);
    }

    public void MouseMove(float x, float y, bool move) => MouseMove(new Vector2(x, y), move);

    public void MouseMove(Vector2 location, bool move)
    {
        if (move)
        {
            OnMouseMove(location - LastMouseLocation);
        }
        LastMouseLocation = location;
    }

    protected abstract void OnScroll(int delta);

    protected abstract void OnMouseMove(Vector2 diff);



    public virtual void CreatePerspective()
    {
        ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, AspectRatio, 1.0f, 16_000_0.0f);
    }

    public abstract void CreateView();

    public abstract Vector3 ViewportPosToVector(Vector2 position);
}

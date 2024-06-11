using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Mathematics;

namespace SyneticLib.Math3D;
public abstract class Camera
{
    public CameraInputs Inputs { get; }

    public Camera()
    {
        Inputs = new CameraInputs();
    }

    public Matrix4 ProjectionMatrix = Matrix4.Identity;
    public Matrix4 ViewMatrix = Matrix4.Identity;
    public Vector3 Up = new Vector3(0,1,0);

    public Vector3 Rotation;
    public Vector3 Position = new Vector3(0, 15, 0);

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

    Vector2 NormalizedDeviceLocation(Vector2 vector)
    {
        return (vector / ScreenSize) * 2f - Vector2.One;
    }

    Vector4 HomogeneousClipCoordinates(Vector2 vector)
    {
        var n = NormalizedDeviceLocation(vector);
        return new Vector4(n.X, -n.Y, -1f, 1f);
    }

    public Ray CastMouseRay() => GetRayCastDirection(LastMouseLocation);

    public Ray GetRayCastDirection(Vector2 screenCordinates)
    {
        var clipCoords = HomogeneousClipCoordinates(screenCordinates);

        var invProjection = ProjectionMatrix.Inverted();
        var invView = ViewMatrix.Inverted();

        var eyeCoords = ( clipCoords * invProjection );
        eyeCoords.Z = -1;
        eyeCoords.W = 0;

        var rayWorld = (eyeCoords * invView).Xyz;

        var direction = rayWorld.Normalized();
        return new Ray(Position, direction);
    }


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

    public virtual void Update(TimeSpan delta)
    {

    }

    public virtual void CreatePerspective()
    {
        ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, AspectRatio, 1.0f, 32_000_0.0f);
    }

    public abstract void CreateView();

    public abstract Vector3 ViewportPosToVector(Vector2 position);
}

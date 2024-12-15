using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Mathematics;

namespace SyneticLib.WinForms.Drawing;

public class GdiCamera
{
    public Vector2 Position;
    public float Scale = 1;

    private Vector2 _screenSize;
    private Vector2 _halfScreenSize;

    private Vector2 lastLocation;

    public Vector2 ScreenSize
    {
        get => _screenSize;
        set
        {
            _screenSize = value;
            _halfScreenSize = value * 0.5f;
        }
    }

    public void MouseScrollEvent(MouseEventArgs e, float scrollFactor)
    {
        var mlocation = new Vector2(e.X, e.Y);
        var oldWorldPos = ScreenToWorldSpace(mlocation);
        if (e.Delta < 0)
            Scale *= scrollFactor;
        else
            Scale /= scrollFactor;

        Scale = Math.Clamp(Scale, 0.125f, 64f);

        var newWorldPos = ScreenToWorldSpace(mlocation);
        Position += oldWorldPos - newWorldPos;
    }

    public void MouseMoveEvent(MouseEventArgs e, bool move)
    {
        var mlocation = new Vector2(e.X, e.Y);
        if (move)
        {
            var oldWorldPos = ScreenToWorldSpace(lastLocation);
            var newWorldPos = ScreenToWorldSpace(mlocation);
            Position += oldWorldPos - newWorldPos;
        }
        lastLocation = mlocation;
    }

    public Vector2 ScreenToWorldSpace(Vector2 screenPos)
    {
        return (screenPos - _halfScreenSize) / Scale + Position;
    }

    public Vector2 WorldToScreenSpace(Vector2 worldPos)
    {
        return (worldPos - Position) * Scale + _halfScreenSize;
    }
}
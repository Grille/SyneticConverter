using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Numerics;

namespace SyneticTool;

class Camera
{
    public Vector3 Position;
    public float Scale;

    private float width, height;
    private float hWidth, hHeight;

    private Vector2 lastLocation;

    public Size ScreenSize
    {
        get => new Size((int)width, (int)height);
        set
        {
            width = value.Width;
            height = value.Height;
            hWidth = width / 2;
            hHeight = height / 2;
        }
    }

    public void MouseScrollEvent(MouseEventArgs e, float scrollFactor)
    {
        var oldWorldPos = ScreenToWorldSpace((Vector2)(PointF)e.Location);
        if (e.Delta > 0)
            Scale *= scrollFactor;
        else
            Scale /= scrollFactor;

        var newWorldPos = ScreenToWorldSpace((Vector2)(PointF)e.Location);
        Position.X += oldWorldPos.X - newWorldPos.X;
        Position.Y += oldWorldPos.Y - newWorldPos.Y;
    }

    public void MouseMoveEvent(MouseEventArgs e, bool move)
    {
        if (move)
        {
            var oldWorldPos = ScreenToWorldSpace(lastLocation);
            var newWorldPos = ScreenToWorldSpace((Vector2)(PointF)e.Location);
            Position.X += oldWorldPos.X - newWorldPos.X;
            Position.Y += oldWorldPos.Y - newWorldPos.Y;
        }
        lastLocation = (Vector2)(PointF)e.Location;
    }

    public Vector3 ScreenToWorldSpace(Vector2 screenPos)
    {
        screenPos.Y *= 2;
        var screwpos = new Vector2((screenPos.X - hWidth) / Scale, (screenPos.Y - height) / Scale);
        var transpos = new Vector2((screwpos.X - screwpos.Y) / 2, (screwpos.Y + screwpos.X) / 2);
        var pos = new Vector3(transpos.X + Position.X, transpos.Y + Position.Y, 0);
        return pos;
    }

    public Vector2 WorldToScreenSpace(Vector3 worldPos)
    {
        var transpos = worldPos - Position;
        var screwpos = new Vector2(transpos.X + transpos.Y, transpos.Y - transpos.X - transpos.Z*2);
        var pos = new Vector2(screwpos.X * Scale + hWidth, screwpos.Y * Scale + height);
        pos.Y /= 2;
        return pos;
    }
}

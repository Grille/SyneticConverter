using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;
using SyneticLib;
namespace SyneticTool;

internal class Renderer
{
    Graphics g;
    Camera camera;
    Scenario scenario;
    public void Render(Graphics g, Camera camera, Scenario scenario)
    {
        this.g = g;
        this.camera = camera;
        this.scenario = scenario;
        DrawCompass(Vector3.Zero);

        var v1 = scenario.Variants[0];
        var mesh = v1.Terrain;

        /*
        foreach (var poly in mesh.Poligons)
        {
            DrawPoly(Pens.Black, mesh.Vertices[poly.X].Position, mesh.Vertices[poly.Y].Position, mesh.Vertices[poly.Z].Position);
        }
        */
        foreach (var prop in v1.PropInstances)
        {
            DrawCompass(prop.Position);
        }
    }

    public void DrawPoly(Pen pen, Vector3 pos0, Vector3 pos1, Vector3 pos2)
    {
        var spos0 = (PointF)camera.WorldToScreenSpace(pos0);
        var spos1 = (PointF)camera.WorldToScreenSpace(pos1);
        var spos2 = (PointF)camera.WorldToScreenSpace(pos2);

        g.DrawLine(pen, spos0, spos1);
        g.DrawLine(pen, spos1, spos2);
        g.DrawLine(pen, spos2, spos0);
    }

    public void DrawCompass(Vector3 pos)
    {
        DrawLine(new Pen(Color.Red, 2), pos + new Vector3(-1, 0, 0), pos + new Vector3(1, 0, 0));
        DrawLine(new Pen(Color.Green, 2), pos + new Vector3(0, -1, 0), pos + new Vector3(0, 1, 0));
        DrawLine(new Pen(Color.Blue, 2), pos + new Vector3(0, 0, -1), pos + new Vector3(0, 0, 1));
    }

    public void DrawLine(Pen pen, Vector3 pos0, Vector3 pos1)
    {
        g.DrawLine(pen, (PointF)camera.WorldToScreenSpace(pos0), (PointF)camera.WorldToScreenSpace(pos1));
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace SyneticLib.Utils;

public class VertexColorCorrector
{
    public static void ConvertC11ToWR2(ref Vertex vertex)
    {
        var black = new Vector3(0.2f);
        float f = vertex.LightColor.Length;
        vertex.LightColor = Vector3.Clamp(vertex.LightColor * f + black * (1 - f), Vector3.Zero, Vector3.One);
        float min = MathF.Min(MathF.Min(vertex.LightColor.X, vertex.LightColor.Y), vertex.LightColor.Z);
        var minvec = new Vector3(min + 0.1f);
        vertex.LightColor = Vector3.ComponentMin(vertex.LightColor, minvec);
        if (min > 0.6)
        {
            vertex.LightColor = vertex.LightColor * 0.25f + new Vector3(0.45f);
        }
    }

    public static void ClampToMin(ref Vector3 color, float offset)
    {
        float min = MathF.Min(MathF.Min(color.X, color.Y), color.Z);
        var minvec = new Vector3(min + offset);
        color = Vector3.ComponentMin(color, minvec);
    }

    public static void ClampToMax(ref Vector3 color, float offset)
    {
        float min = MathF.Max(MathF.Max(color.X, color.Y), color.Z);
        var minvec = new Vector3(min + offset);
        color = Vector3.ComponentMax(color, minvec);
    }
}

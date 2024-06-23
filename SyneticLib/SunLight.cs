using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace SyneticLib;
public struct SunLight
{
    public Vector3 Color;
    public Vector3 Direction;

    public SunLight(Vector3 color, Vector3 direction)
    {

        Color = color; Direction = direction;

    }

    public static readonly SunLight Default = new SunLight(Vector3.One, new Vector3(0,-1,0));
}

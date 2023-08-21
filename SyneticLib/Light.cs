using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using System.Drawing;

namespace SyneticLib;
public class Light : Ressource
{
    public Vector3 Position { get; }
    public Color Color { get; }

    public Light(string name, Vector3 position, Color color) : base(name)
    {
        Position = position;
        Color = color;
    }
}

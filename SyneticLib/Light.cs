﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using System.Drawing;

namespace SyneticLib;
public class Light : SyneticObject
{
    public Vector3 Position { get; }
    public Color Color { get; }

    public Light(Vector3 position, Color color)
    {
        Position = position;
        Color = color;
    }
}

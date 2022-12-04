﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SyneticLib; 
public class PropInstance : Ressource
{
    public PropClass Class;
    public bool InShadow = false;
    public Vector3 Position;
    public float Angle;
    public float Scale;

    public PropInstance(Ressource parent, PropClass @class) : base(parent, parent.ChildPath("PropInstance"), PointerType.Virtual)
    {
        Class = @class;
    }
}

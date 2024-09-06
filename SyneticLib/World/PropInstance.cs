using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SyneticLib.World;
public class PropInstance : SyneticObject
{
    public PropClass Class;
    public bool InShadow = false;
    public Vector3 Position;
    public float Angle;
    public float Scale;

    public PropInstance(PropClass @class)
    {
        Class = @class;
    }
}

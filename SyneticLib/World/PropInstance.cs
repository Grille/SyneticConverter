using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace SyneticLib.World;
public class PropInstance : SyneticObject
{
    public string Class;
    public bool InShadow = false;
    public Vector3 Position;
    public Matrix3 Matrix;

    public PropInstance(string @class)
    {
        Class = @class;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticConverter;
public struct Transform
{
    public Transform3 X;
    public Transform2 Y;
    public Transform3 Z;

    public override string ToString()
    {
        return $"{X.Rotate} {X.Scale} {X.Move} {Y.Rotate} {Z.Rotate} {Z.Scale} {Z.Move}";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.World;
public class PropClass : SyneticObject
{
    public int AnimationMode;
    public int ColliShape;

    //public Model Model;

    public PropClass(string name)
    {
        Name = name;
    }
}

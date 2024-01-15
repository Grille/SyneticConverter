using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class PropClass : SyneticObject
{
    public string Name;

    public int AnimationMode;
    public int ColliShape;

    public Mesh Mesh;

    public ModelDirectory AssignedModels;

    public PropClass(string name) : base(name)
    {

    }
}

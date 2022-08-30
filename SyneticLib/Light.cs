using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace SyneticLib;
public class Light : Ressource
{
    public Vector3 Position;
    public Color Color;

    public Light(Ressource parent) : base(parent, parent.ChildPath("Light"), PointerType.Virtual)
    {
    }

    protected override void OnLoad()
    {
        //throw new NotImplementedException();
    }

    protected override void OnSave()
    {
        //throw new NotImplementedException();
    }

    protected override void OnSeek()
    {
        //throw new NotImplementedException();
    }
}

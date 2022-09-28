using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class Wheel : Ressource
{
    Mesh Mesh;

    public Wheel(Car parent) : base(parent, parent.ChildPath("Wheel"))
    {
    }

    protected override void OnLoad()
    {
        throw new NotImplementedException();
    }

    protected override void OnSave()
    {
        throw new NotImplementedException();
    }

    protected override void OnSeek()
    {
        throw new NotImplementedException();
    }
}

using System;
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

    public PropInstance(Ressource parent, PropClass @class) : base(parent, PointerType.Virtual)
    {
        Class = @class;
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

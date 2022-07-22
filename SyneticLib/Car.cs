using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class Car : Ressource
{
    public override DataState DataState => throw new NotImplementedException();

    public override void CopyTo(string path)
    {
        throw new NotImplementedException();
    }

    public override void LoadAll()
    {
        throw new NotImplementedException();
    }

    public override void SeekAll()
    {
        throw new NotImplementedException();
    }
}

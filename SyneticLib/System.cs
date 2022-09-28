using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;



public class Global : Ressource
{
    public readonly static Global Instance = new Global();

    public Queue<Ressource> LoadingQueue;

    private Global() : base(null, "System", PointerType.Virtual)
    {
    }


    protected override void OnLoad()
    {
        throw new NotImplementedException();
    }

    protected override void OnSave() => throw new NotImplementedException();

    protected override void OnSeek()
    {
        throw new NotImplementedException();
    }
}

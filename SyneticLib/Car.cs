using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class Car : Ressource
{
    public Model Model;
    public TerrainMaterialList Materials;

    public Car(GameFolder parent, string path) : base(parent, path, PointerType.Directory)
    {
        Model = new Model(parent, ChildPath(FileName + ".mox"));
    }

    protected override void OnLoad()
    {
        Model.Load();
    }

    protected override void OnSave()
    {
        throw new NotImplementedException();
    }

    protected override void OnSeek()
    {
        Model.Seek();
    }
}

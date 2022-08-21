using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class Car : Ressource
{
    public Mesh Mesh;
    public MaterialList Materials;

    public Car(GameFolder parent, string path) : base(parent, PointerType.Directory)
    {
        SourcePath = path;

        Mesh = new Mesh(parent, ChildPath(FileName + ".mox"));
    }

    protected override void OnLoad()
    {
        Mesh.Load();
    }

    protected override void OnSave()
    {
        throw new NotImplementedException();
    }

    protected override void OnSeek()
    {
        Mesh.Seek();
    }
}

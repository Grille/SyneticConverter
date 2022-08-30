using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class PropClass : Ressource
{
    public string Name;

    public int AnimationMode;
    public int ColliShape;

    public Mesh Mesh;

    public PropClass(ScenarioVariant parent, string name) : base(parent, parent.ChildPath(name), PointerType.Virtual)
    {
        Name = FileName;
        //Mesh = new(Materials);
    }

    protected override void OnLoad()
    {

    }

    protected override void OnSave()
    {

    }

    protected override void OnSeek()
    {

    }
}

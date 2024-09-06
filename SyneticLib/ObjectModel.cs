using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public class ObjectModel : Model
{
    public ObjectModel(MeshSegment mesh, ModelMaterialRegion[] regions) : base(mesh, regions)
    {
    }
}

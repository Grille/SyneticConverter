using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;

namespace SyneticLib.IO;
public class MaterialsMtlSerializer : FileSerializer<MtlFile, ModelMaterial[]>
{
    public override ModelMaterial[] OnDeserialize(MtlFile file)
    {
        throw new NotImplementedException();
    }

    protected override void OnSerialize(MtlFile file, ModelMaterial[] value)
    {
        throw new NotImplementedException();
    }
}

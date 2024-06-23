using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;

namespace SyneticLib.IO;
public class ModelMoxSerializer : FileSerializer<MoxFile, Model>
{
    public override Model OnDeserialize(MoxFile file)
    {
        throw new NotImplementedException();
    }

    protected override void OnSerialize(MoxFile file, Model value)
    {
        throw new NotImplementedException();
    }
}

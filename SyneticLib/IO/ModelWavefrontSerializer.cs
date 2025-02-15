using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;
using SyneticLib.Files.Extra;

namespace SyneticLib.IO;
public class ModelWavefrontSerializer : FileSerializer<WavefrontObjFile, Model>
{
    protected override Model OnDeserialize(WavefrontObjFile file)
    {
        throw new NotImplementedException();
    }

    protected override void OnSerialize(WavefrontObjFile file, Model value)
    {
        throw new NotImplementedException();
    }
}

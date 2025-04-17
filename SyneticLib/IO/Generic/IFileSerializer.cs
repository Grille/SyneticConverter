using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.IO.Generic;
public interface IFileSerializer<TObj> : ISerializer<TObj>
{
    public void Serialize(Stream stream, TObj value);

    public TObj Deserialize(Stream stream);
}

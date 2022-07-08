using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.IO.Synetic.Files;
public interface IIndexData
{
    public ushort[] Indices { get; set; }
}

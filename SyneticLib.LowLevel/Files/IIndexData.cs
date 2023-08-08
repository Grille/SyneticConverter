using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.LowLevel.Files;
public interface IIndexData
{
    public IndexTriangle[] Indices { get; set; }
}

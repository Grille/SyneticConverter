using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Files;
public interface IIndexData
{
    public IdxTriangleInt32[] Indices { get; }
}

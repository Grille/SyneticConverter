using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public interface IDataPointer
{
    public string SrcPath { get; set; }

    public PointerState PointerState { get; }

    public bool FileExists { get; }

    public void CopyTo(string dstPath);
}

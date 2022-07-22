using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;
public enum PointerState
{
    None,
    Invalid,
    Exists,
    Loaded,
    Outdated,
    Virtual,
}

public enum DataState
{
    None,
    Loaded,
    Changed,
    Error,
}

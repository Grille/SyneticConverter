using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

public enum GameVersion
{
    Invalid = -2,
    Auto = -1,
    NICE,
    NICE2,
    MBTR,
    MBWR,
    WR2,
    C11,
    CTP,
    CT2,
    FVR,
    CT3,
    CT4,
    CT5,
}

public enum PointerState
{
    None,
    Exists,
    Invalid,
}

public enum PointerType
{
    None,
    Virtual,
    Directory,
    File,
}

public enum DataState
{
    None,
    Seeked,
    Loaded,
    Changed,
    Error,
}

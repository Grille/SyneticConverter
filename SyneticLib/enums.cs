﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib;

public enum GameVersion
{
    Invalid = -2,
    Auto = -1,
    NICE1,
    NICE2,
    MBTR,
    WR1,
    WR2,
    C11,
    CT1,
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

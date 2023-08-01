﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib.IO;
public static class SoundImport
{
    public static Sound Load(string path) 
    { 
        var name = Path.GetFileNameWithoutExtension(path);
        var type = Path.GetExtension(path);
        var buffer = File.ReadAllBytes(path);
        return new Sound(name, type, buffer);
    }
}

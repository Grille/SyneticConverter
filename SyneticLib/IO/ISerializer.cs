﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.IO;
public interface ISerializer<TObj>
{
    public TObj Load(string path);

    public void Save(string path, TObj obj);
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.IO;
public abstract class MeshExporter
{
    protected Mesh target;
    protected string path;

    public MeshExporter(Mesh target)
    {
        this.target = target;
        //path = target.RootDir;
    }

    protected abstract void OnSave();

    public void Save() => OnSave();

    public void Save(string path)
    {
        this.path = path;
        Save();
    }
}

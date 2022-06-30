﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticConverter;
public class Texture
{
    public string Name;
    public int Id = 0;
    public byte[] Data;
    public bool AlphaClip = false;
    public float Specular = 0;

    public Texture(string name)
    {
        Name = name;
    }

    public void ImportPtx(string path)
    {

    }
}

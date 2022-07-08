﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGL.IO;

namespace SyneticLib.IO.Synetic.Files;
internal class IdxFile : SyneticBinFile, IIndexData
{
    public ushort[] Indices { get; set; }

    public override void Read(BinaryViewReader br)
    {
        Indices = br.ReadArray<ushort>(LengthPrefix.Int32);
    }

    public override void Write(BinaryViewWriter bw)
    {
        bw.WriteArray(Indices, LengthPrefix.Int32);
    }
}
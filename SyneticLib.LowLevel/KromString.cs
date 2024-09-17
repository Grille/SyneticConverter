using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.IO;
using Grille.IO.Interfaces;

namespace SyneticLib;

public static class KromString
{

    public static string ReadKromString(this BinaryViewReader br)
    {
        string value = br.ReadString(LengthPrefix.UInt16);
        if (br.ReadByte() != 0)
        {
            throw new InvalidDataException();
        }
        return value;
    }

    public static void WriteKromString(this BinaryViewWriter bw, string value)
    {
        bw.WriteString(value, LengthPrefix.UInt16);
        bw.WriteByte(0);
    }
}

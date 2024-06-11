using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib.LowLevel.Compression;
public static class SynCompressor
{
    public static void Decompress(Stream src, Stream dst, int size)
    {
        long endpos = src.Position + size;
        if (endpos > src.Length)
            throw new ArgumentOutOfRangeException();

        while (src.Position < endpos)
        {
            byte flagByte = (byte)src.ReadByte();

            for (int i = 0; i < 8; i++)
            {
                // false: Take from behind; true: Take that
                bool flag = (byte)(flagByte >> i & 1) == 1;

                if (flag)
                {
                    dst.WriteByte((byte)src.ReadByte());
                }
                else
                {
                    DecompressBlock(src, dst);
                }

            }
        }

        long diff = src.Position - endpos;

        if (diff != 0)
        {
            throw new InvalidDataException($"Invalid position (result:{src.Position} != expected:{endpos}) diff:{diff}");
        }
    }

    static void DecompressBlock(Stream src, Stream dst)
    {
        //1byte + 4bits from 2nd byte  Length is only last 4bits+3
        byte head0 = (byte)src.ReadByte();
        byte head1 = (byte)src.ReadByte();

        int dist = head0 | (head1 & 0xF0) >> 8;
        int leng = (head1 & 0x0F) + 3;

        dst.Position += leng;

        /*
        if (idst > (18 + addv + 4096))
            addv += 4096;

        for (int i = 0; i < leng; i++)
        {
            if (dist >= (idst - addv))
            {
                if ((18 + i + dist + addv - 4096) <= 0)
                    dst[idst + i] = 32;
                else
                    dst[idst + i] = dst[18 + i + dist + addv - 4096];
            }
            else
            {
                if ((18 + i + dist + addv) > (idst + i))
                    dst[idst + i] = dst[18 + i + dist + addv - 4096];
                else
                    dst[idst + i] = dst[18 + i + dist + addv];
            }
        }
        */
    }


}

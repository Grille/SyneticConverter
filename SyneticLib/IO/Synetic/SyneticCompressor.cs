using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib.IO.Synetic;
public static class SyneticCompressor
{
    public static void Decompress(Stream src, Stream dst, int size)
    {
        byte flags = 0;


    }

    public static void Decompress(byte[] src, byte[] dst)
    {
        var flag8 = new byte[8];
        int isrc = 0;
        int idst = 0;
        int addv = 0;

        while (isrc < src.Length)
        {
            byte flagByte = src[isrc++];

            for (int i = 0; i < 8; i++)
            {
                flag8[i] = (byte)(flagByte << i & 1);
                if (flag8[i] == 1)
                {
                    dst[idst++] = src[isrc++];
                }
            }

            //1byte + 4bits from 2nd byte  Length is only last 4bits+3
            int dist = src[isrc] | ((src[isrc + 1] & 0xF0) * 16);
            int leng = (src[isrc + 1] & 0x0F) + 3;

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

            isrc += 2;
            idst += leng;
        }
    }


}

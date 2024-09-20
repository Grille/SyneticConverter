using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib.LowLevel.Compression;
public static class SynDecompressor
{
    public static void Decompress(byte[] src, byte[] dst)
    {
        byte[] d = src;
        byte[] c = dst;

        int ci = 0;
        int curChr = 0; // current byte
        int addv = 0;

        do
        {
            byte x = d[ci++];

            for (int i = 0; i < 8; i++)
            {
                if (ci + 1 >= src.Length)
                {
                    break;
                }

                // 0 - Take from behind
                // 1 - Take that
                bool flag = ((x >> i) & 1) == 1;

                if (flag)
                {
                    c[curChr] = d[ci];
                    curChr++;
                    ci++;
                }
                else
                {
                    int dist = d[ci] + ((d[ci + 1] & 0xF0) * 16); // 1 byte + 4 bits from 2nd byte
                    int leng = (d[ci + 1] & 0x0F) + 3;

                    if (curChr > (18 + addv + 4096))
                        addv += 4096;

                    for (int k = 0; k < leng; k++)
                    {
                        if (dist >= (curChr - addv))
                        {
                            if ((18 + k + dist + addv - 4096) <= 0)
                                c[curChr + k] = (byte)' '; // If overlap backward
                            else
                                c[curChr + k] = c[18 + k + dist + addv - 4096];
                        }
                        else
                        {
                            if ((18 + k + dist + addv) > (curChr + k))
                                c[curChr + k] = c[18 + k + dist + addv - 4096];
                            else
                                c[curChr + k] = c[18 + k + dist + addv];
                        }
                    }

                    curChr += leng;
                    ci += 2;
                }
            }
        }
        while (ci < src.Length);
    }
}

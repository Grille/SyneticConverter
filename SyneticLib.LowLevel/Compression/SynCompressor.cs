using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib.LowLevel.Compression;
public static class SynCompressor
{
    public static void Decompress(byte[] src, byte[] dst)
    {
        byte[] d = src;
        byte[] c = dst;

        int ci = 0;
        int curChr = 0; // current byte
        int addv = 0;
        int[] flag = new int[8];

        do
        {

            // Ensure ci + 1 is within the bounds of the array
            if (ci + 1 >= d.Length)
            {
                Console.WriteLine("Attempted to read beyond the end of the data array.");
                break; // Exit the loop or handle the situation appropriately
            }

            byte x = d[ci];
            ci++;
            if (x >= 128) { x -= 128; flag[7] = 1; } else flag[7] = 2;  // 2 means 0
            if (x >= 64) { x -= 64; flag[6] = 1; } else flag[6] = 2;    // 1 - Take that
            if (x >= 32) { x -= 32; flag[5] = 1; } else flag[5] = 2;    // 2 - Take from behind
            if (x >= 16) { x -= 16; flag[4] = 1; } else flag[4] = 2;
            if (x >= 8) { x -= 8; flag[3] = 1; } else flag[3] = 2;
            if (x >= 4) { x -= 4; flag[2] = 1; } else flag[2] = 2;
            if (x >= 2) { x -= 2; flag[1] = 1; } else flag[1] = 2;
            if (x >= 1) { /* x -= 1; */ flag[0] = 1; } else flag[0] = 2;

            for (int i = 0; i < 8; i++)
            {
                if (flag[i] == 1)
                {
                    c[curChr] = d[ci];
                    curChr++;
                    ci++;
                }
                else
                {
                    if (ci + 1 >= src.Length)
                        break;

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

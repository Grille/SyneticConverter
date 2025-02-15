using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files;

namespace SyneticLib.Utils;

public static class RandObjToBgraConverter
{
    public static BgraColor[] Convert(string filePath, int size)
    {
        var ro0 = new Ro0File();
        ro0.Load(filePath);
        return Convert(ro0, size);
    }

    public static BgraColor[] Convert(Ro0File ro0, int size)
    {
        var bgra = new BgraColor[size * size];

        var grass = ro0.Grass;

        float minX = 0, maxX = 0;
        float minZ = 0, maxZ = 0;

        for (int i = 0; i < grass.Length; i++)
        {
            minX = Math.Min(minX, grass[i].Position.X);
            maxX = Math.Max(maxX, grass[i].Position.X);
            minZ = Math.Min(minZ, grass[i].Position.Z);
            maxZ = Math.Max(maxZ, grass[i].Position.Z);
        }

        float scaleX = (1 / (maxX - minX)) * (size-1);
        float scaleY = (1 / (maxZ - minZ)) * (size-1);

        for (int i = 0; i < grass.Length; i++)
        {
            float x = (grass[i].Position.X - minX) * scaleX;
            float z = (grass[i].Position.Z - minZ) * scaleY;
            int index = (int)x + (int)z * size;

            bgra[index] = BgraColor.FromNormalizedRgbVector3(grass[i].Color);
        }

        return bgra;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using SN = System.Numerics;
using TK = OpenTK.Mathematics;
using System.Runtime.InteropServices;
using static SyneticLib.Utils.VectorUtils;

namespace SyneticLib.Utils;
public static class VectorUtils
{
    [StructLayout(LayoutKind.Sequential, Size = 4 * 3)]
    public struct DataCast3
    {
        public static implicit operator DataCast3(SN.Vector3 inp)
        {
            return Unsafe.As<SN.Vector3, DataCast3>(ref inp);
        }

        public static implicit operator DataCast3(TK.Vector3 inp)
        {
            return Unsafe.As<TK.Vector3, DataCast3>(ref inp);
        }

        public static implicit operator SN.Vector3(DataCast3 inp)
        {
            return Unsafe.As<DataCast3, SN.Vector3>(ref inp);
        }

        public static implicit operator TK.Vector3(DataCast3 inp)
        {
            return Unsafe.As<DataCast3, TK.Vector3>(ref inp);
        }


    }
}

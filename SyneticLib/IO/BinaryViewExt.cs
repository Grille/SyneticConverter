using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGL.IO;
using System.IO;
using SyneticLib.IO.Synetic;

namespace SyneticLib.IO;
public static class BinaryViewSyneticUtils
{
    public static void BeginSynSection(this BinaryViewReader br, long length)
    {
        var src = br.PeakStream;
        br.StreamStack.Create();
        var dst = br.PeakStream;
        SynCompressor.Decompress(src, dst, (int)length);
        dst.Seek(0, SeekOrigin.Begin);
    }

    public static void EndSynSection(this BinaryViewReader br)
    {
        br.StreamStack.DisposePeak();
    }

    public static void BeginSynSection(this BinaryViewWriter bw)
    {
        throw new NotImplementedException();
    }

    public static void EndSynSection(this BinaryViewWriter bw)
    {
        throw new NotImplementedException();
    }
}

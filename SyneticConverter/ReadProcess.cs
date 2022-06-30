using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGL.IO;
using SyneticConverter;

namespace SyneticConverter;
public class ReadProcess
{
    public GameVersion Target;
    public BinaryViewReader Reader;

    List<string> Info;
    List<string> Warnings;
    List<string> Error;

    public T Load<T>(string path) where T : SyneticFile, new()
    {
        var obj = new T();
        obj.Load(path);
        return obj;
    }
}

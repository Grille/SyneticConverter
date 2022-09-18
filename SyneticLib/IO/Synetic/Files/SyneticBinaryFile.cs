using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using GGL.IO;

namespace SyneticLib.IO.Synetic.Files;
public abstract class SyneticBinaryFile : IViewObject
{
    private string _path = "";
    public string Path
    {
        get => _path; 
        set
        {
            _path = value;
            FileName = System.IO.Path.GetFileName(value);
        }
    }

    public string FileName { get; private set; }

    public abstract void ReadFromView(BinaryViewReader br);

    public abstract void WriteToView(BinaryViewWriter bw);

    public void ReadFromArchive(SynArchive archive)
    {
        if (!archive.FileExists(FileName))
            throw new FileNotFoundException($"file '{Path}' not found in syn", Path);

        using var br = new BinaryViewReader(archive.GetFile(FileName), true);
        br.ReadToIView(this);
    }

    public bool Exists
    {
        get => File.Exists(Path);
    }

    public void Load(string path = null)
    {
        if (path != null)
            Path = path;

        if (!File.Exists(Path))
            throw new FileNotFoundException($"file '{Path}' not found", Path);

        using var br = new BinaryViewReader(Path);
        br.ReadToIView(this);
    }

    public void Save(string path = null)
    {
        if (path != null)
            Path = path;

        using var bw = new BinaryViewWriter(Path);
        bw.WriteIView(this);
    }


}

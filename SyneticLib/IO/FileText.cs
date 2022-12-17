using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGL.IO;

namespace SyneticLib.IO;
public abstract class FileText
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

    public bool Exists => File.Exists(Path);

    public abstract void ReadFromFile(StreamReader r);

    public abstract void WriteToFile(StreamWriter w);

    public void Load(string path = null)
    {
        if (path != null)
            Path = path;

        if (!File.Exists(Path))
            throw new FileNotFoundException($"file '{Path}' not found", Path);

        using var r = new StreamReader(Path);
        ReadFromFile(r);
    }

    public void Save(string path = null)
    {
        if (path != null)
            Path = path;

        using var w = new StreamWriter(Path);
        WriteToFile(w);
    }
}

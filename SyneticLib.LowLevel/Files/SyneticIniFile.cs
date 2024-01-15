using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GGL.IO;

namespace SyneticLib.LowLevel.Files;
public abstract class SyneticIniFile
{
    public class Section : Dictionary<string, string>
    {
        public string Name;
    }

    public Section Head = new();
    public List<Section> Sections = new();

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

    public void ReadFromFile(StreamReader r)
    {
        Head.Clear();
        Sections.Clear();

        Section section = Head;
        while (!r.EndOfStream)
        {
            var line = r.ReadLine();
            var split = line.Split(' ', 2);
            if (split.Length != 2)
                continue;

            var key = split[0].Trim();
            var value = split[1].Trim();

            switch (key)
            {
                case "#":
                    {
                        section = new Section();
                        section.Name = value;
                        Sections.Add(section);
                        break;
                    }
                default:
                    {
                        section.Add(key, value);
                        break;
                    }
            }
        }

        OnRead();
    }

    public void WriteToFile(StreamWriter w)
    {
        //Head.Clear();
        //Sections.Clear();

        OnWrite();

        if (Head.Count > 0)
        {
            foreach (var pair in Head)
            {
                w.WriteLine($"{pair.Key} {pair.Value}");
            }
            w.WriteLine();
        }

        foreach (var section in Sections)
        {
            w.WriteLine($"# {section.Name}");
            foreach (var pair in section)
            {
                w.WriteLine($"{pair.Key} {pair.Value}");
            }
            w.WriteLine();
        }
    }

    protected abstract void OnRead();

    protected abstract void OnWrite();

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

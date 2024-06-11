using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grille.IO;

namespace SyneticLib.Files.Common;
public abstract class SyneticIniFile : BaseFile
{
    public class Section : Dictionary<string, string>
    {
        public string Name;

        public Section(string name)
        {
            Name = name;
        }
    }

    public Section Head;
    public List<Section> Sections;

    public SyneticIniFile()
    {
        Head = new("Head");
        Sections = new();
    }


    public override sealed void Deserialize(Stream stream)
    {
        using var br = new StreamReader(stream, leaveOpen: true);
        Deserialize(br);
    }

    public override sealed void Serialize(Stream stream)
    {
        using var bw = new StreamWriter(stream, leaveOpen: true);
        Serialize(bw);
    }

    public void Deserialize(StreamReader r)
    {
        Head.Clear();
        Sections.Clear();

        Section section = Head;
        while (!r.EndOfStream)
        {
            var line = r.ReadLine();
            if (line == null)
                break;

            var split = line.Split(' ', 2);
            if (split.Length != 2)
                continue;

            var key = split[0].Trim();
            var value = split[1].Trim();

            switch (key)
            {
                case "#":
                    {
                        section = new Section(value);
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

    public void Serialize(StreamWriter w)
    {
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
}

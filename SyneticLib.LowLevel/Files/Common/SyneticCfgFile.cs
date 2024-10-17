using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.IO;
using System.Runtime.CompilerServices;

namespace SyneticLib.Files.Common;
public abstract class SyneticCfgFile<T> : TextFile where T : Dictionary<string, string>, new()
{
    public T Head { get; }
    public Dictionary<string, T> Sections { get; }

    public SyneticCfgFile()
    {
        Head = new();
        Sections = new();
    }

    public override void Deserialize(StreamReader r)
    {
        Head.Clear();
        Sections.Clear();

        var section = Head;
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
                    section = new T();
                    Sections[value] = section;
                    break;
                }
                default:
                {
                    section[key] = value;
                    break;
                }
            }
        }
    }

    public override void Serialize(StreamWriter w)
    {
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
            w.WriteLine($"# {section.Key}");
            foreach (var entry in section.Value)
            {
                w.WriteLine($"{entry.Key} {entry.Value}");
            }
            w.WriteLine();
        }
    }
}

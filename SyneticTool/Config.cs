using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticTool;

internal class Config
{
    ConfigSection UsedSection;
    List<ConfigSection> ConfigSections;

    public void UseSection(string name)
    {

    }

    public void UseSection()
    {

    }

    public void SetValue(string key, string value)
    {

    }

    public string GetValue(string key)
    {
        return "";
    }

    public void Load(string path)
    {
        var lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[0].Split(new[] { "//","#" }, StringSplitOptions.TrimEntries)[0];

        }
    }
}

class ConfigSection
{
    public string Name = "_";
    public bool AllowDuplicateKeys = false;
    public List<ConfigEntry> Entries;
}

class ConfigEntry
{
    public string Key;
    public string Value;
    public string Default;
    public int Position;
    public bool Exists;
}



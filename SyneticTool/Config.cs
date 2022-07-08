using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace SyneticTool;

internal class Config
{
    string path;
    ConfigSection UsedSection;
    List<ConfigSection> ConfigSections;

    public Config(string path)
    {
        this.path = path;
    }

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

    public bool Exists()
    {
        return File.Exists(path);
    }

    public void Load()
    {
        var lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[0].Trim();

            if (line.Length == 0)
                continue;

            if (line[0] == '[')
            {

            }

        }
    }

    public void Save()
    {
        var sb = new StringBuilder();

        File.WriteAllText(path, sb.ToString());
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



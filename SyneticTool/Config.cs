using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Reflection;
using System.Xml.Linq;
using GGL.IO;
using SyneticLib;

namespace SyneticTool;

public class Config : IViewObject
{
    public GameDirectoryList Games;
    string path;
    public Config(string path)
    {
        this.path = path;
        Games = new();
    }

    public void ReadFromView(BinaryViewReader br)
    {
        br.Encoding = Encoding.UTF8;

        int Count = br.ReadInt32();
        for (int i = 0; i < Count; i++)
            Games.Add(new(br.ReadString(), GameDirectory.ParseGameVersion(br.ReadString())));
    }

    public void WriteToView(BinaryViewWriter bw)
    {
        bw.Encoding = Encoding.UTF8;

        bw.WriteInt32(Games.Count);
        for (int i = 0; i < Games.Count; i++)
        {
            var game = Games[i];
            bw.WriteString(game.SourcePath);
            bw.WriteString(game.Version.ToString());
        }
    }

    public bool TryLoad()
    {
        if (File.Exists(path))
        {
            try
            {
                Load();
            }
            catch
            {
                return false;
            }
            return true;
        }
        return false;
    }

    public void Load()
    {
        using var br = new BinaryViewReader(path);
        ReadFromView(br);
    }

    public void Save()
    {
        using var br = new BinaryViewWriter(path);
        WriteToView(br);
    }

    public record class Game(string Path, string Format);
}




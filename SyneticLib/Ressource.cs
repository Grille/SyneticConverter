using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;
public abstract class Ressource { 

    public GameVersion Version { get; set; }
    public Ressource Parent { get; set; }
    public Ressource[] Children;

    public Ressource(Ressource parent, PointerType type = PointerType.Virtual)
    {
        PointerType = type;
        Parent = parent;

        if (parent != null)
        {
            Version = parent.Version;
            TargetVersion = parent.TargetVersion;
        }
    }

    string _path;
    public string SourcePath
    {
        get => _path; set
        {
            if (_path == value)
                return;

            _path = value;
            UpdatePointer();
        }
    }

    public string TargetPath { get; set; }
    public GameVersion TargetVersion { get; set; }

    public PointerState PointerState { get; protected set; } = PointerState.None;
    public PointerType PointerType { get; protected set; }
    public DataState DataState { get; set; }


    public bool DirectoryExists => Directory.Exists(SourcePath);

    public bool FileExists => File.Exists(SourcePath);

    public string Extension => Path.GetExtension(SourcePath);

    public string FileName => Path.GetFileNameWithoutExtension(SourcePath);

    public string ChildPath(string path)
    {
        return Path.Combine(SourcePath, path);
    }


    private void UpdatePointer()
    {
        if (PointerType == PointerType.Virtual || SourcePath == null)
        {
            PointerState = PointerState.None;
        }
        else if (PointerType == PointerType.Directory)
        {
            PointerState = DirectoryExists ? PointerState.Exists : PointerState.Invalid;
        }
        else if (PointerType == PointerType.File)
        {
            PointerState = FileExists ? PointerState.Exists : PointerState.Invalid;
        }
    }

    protected abstract void OnLoad();

    protected abstract void OnSave();

    protected abstract void OnSeek();

    public bool NeedSeek => !(DataState == DataState.Seeked || DataState == DataState.Loaded);

    public bool NeedLoad => DataState != DataState.Loaded;

    public void Load()
    {
        if (PointerType == PointerType.Virtual)
            throw new InvalidOperationException("Ressource is Virtual, Load/Save is handled by parent.");

        if (NeedSeek)
            Seek();

        OnLoad();

        DataState = DataState.Loaded;
    }

    public void Save()
    {
        OnSave();
    }

    public void Save(string path)
    {
        TargetPath = path;
        OnSave();
    }

    public void Seek()
    {
        OnSeek();

        DataState = DataState.Seeked;
    }

    public void SaveAs(string path, GameVersion format)
    {
        TargetPath = path;
        TargetVersion = format;
        Save();
    }
}

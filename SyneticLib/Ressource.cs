using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;
public abstract class Ressource {

    private static int ContentIDCounter;
    public int ContentID { get; private set; }
    public GameVersion Version { get; set; }
    public Ressource Parent { get; set; }
    public Ressource[] Children;

    public ProgressLogger ProgressInfo { get; set; }

    public Ressource(Ressource parent, string path, PointerType type = PointerType.Virtual)
    {
        ProgressInfo = new();

        PointerType = type;
        SourcePath = path;
        Parent = parent;

        if (parent != null)
        {
            Version = parent.Version;
            TargetVersion = parent.TargetVersion;
        }

        GetContentID();
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

    public void GetContentID()
    {
        ContentIDCounter++;
        ContentID = ContentIDCounter;
    }

    public string TargetPath { get; set; }
    public GameVersion TargetVersion { get; set; }

    public PointerState PointerState { get; protected set; } = PointerState.None;
    public PointerType PointerType { get; protected set; }
    public DataState DataState { get; set; }


    public bool DirectoryExists => Directory.Exists(SourcePath);

    public bool FileExists => File.Exists(SourcePath);

    public string FileExtension => Path.GetExtension(SourcePath);

    public string FileName => Path.GetFileNameWithoutExtension(SourcePath);

    public string FileNameExt => Path.GetFileName(SourcePath);

    public string ChildPath(string path)
    {
        return Path.Combine(SourcePath, path);
    }


    public void UpdatePointer()
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

    protected virtual void OnLoad()
    {
        throw new NotImplementedException();
    }

    protected virtual void OnSave()
    {
        throw new NotImplementedException();
    }

    protected virtual void OnSeek()
    {
        throw new NotImplementedException();
    }

    public bool NeedSeek => !(DataState == DataState.Seeked || DataState == DataState.Loaded);

    public bool NeedLoad => DataState != DataState.Loaded;

    public void Load()
    {
        AssertPointerIsValid();

        if (NeedSeek)
            Seek();

        if (Parent != null)
            Parent.ProgressInfo.Use(ProgressInfo);

        try
        {
            OnLoad();
        }
        finally
        {
            if (Parent != null)
                Parent.ProgressInfo.Free();
        }
    }

    public void Save()
    {
        AssertPointerIsValid();

        OnSave();
    }

    public void Save(string path)
    {
        TargetPath = path;
        OnSave();
    }

    public void TrySeek()
    {
        UpdatePointer();

        if (PointerType == PointerType.Virtual)
            throw new InvalidOperationException("Ressource is Virtual, Load/Save & Seek is handled by parent.");

        if (PointerState != PointerState.Exists)
        {
            DataState = DataState.None;
            return;
        }

        OnSeek();

        DataState = DataState.Seeked;
    }

    public void Seek()
    {
        AssertPointerIsValid();

        OnSeek();

        DataState = DataState.Seeked;
    }

    private void AssertPointerIsValid()
    {
        UpdatePointer();

        if (PointerType == PointerType.Virtual)
            throw new InvalidOperationException("Ressource is Virtual, Load/Save & Seek is handled by parent.");

        if (PointerState != PointerState.Exists)
            throw new IOException($"Pointed Ressource '{SourcePath}' dont Exists.");
    }

    public void SaveAs(string path, GameVersion format)
    {
        TargetPath = path;
        TargetVersion = format;
        Save();
    }

    public virtual (int value, string msg) PullProgress()
    {
        throw new NotImplementedException();
    }
}

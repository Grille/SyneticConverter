using Grille.IO;
using SyneticLib.LowLevel.Compression;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Files.Common;

public abstract class BaseFile
{
    private string? _path;
    public string? Path
    {
        get => _path;
        set
        {
            _path = value;
            FileName = System.IO.Path.GetFileName(value);
        }
    }

    public bool Exists => File.Exists(Path);

    public string? FileName { get; private set; }

    public abstract void Deserialize(Stream stream);

    public abstract void Serialize(Stream stream);

    public void Validate()
    {
        OnValidate();
    }

    protected virtual void OnValidate()
    {

    }

    public void Load(string path)
    {
        Path = path;
        Load();
    }

    public void Save(string path)
    {
        Path = path;
        Save();
    }


    public void Load()
    {
        AssertPathNotNull();

        if (!File.Exists(Path))
            throw new FileNotFoundException($"file '{Path}' not found", Path);

        using var stream = File.OpenRead(Path);
        Deserialize(stream);
    }

    public void Save()
    {
        AssertPathNotNull();

        using var stream = File.Create(Path);
        Serialize(stream);
    }

    [MemberNotNull(nameof(Path), nameof(FileName))]
    protected void AssertPathNotNull()
    {
        if (Path == null)
            throw new InvalidOperationException("Path must not be null.");

        if (FileName == null)
            throw new InvalidOperationException("FileName must not be null.");

    }

}

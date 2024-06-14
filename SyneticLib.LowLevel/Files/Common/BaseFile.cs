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
    public abstract void Deserialize(Stream stream);

    public abstract void Serialize(Stream stream);

    public void Validate()
    {
        OnValidate();
    }

    protected virtual void OnValidate()
    {

    }

    public void Load(string Path)
    {
        if (!File.Exists(Path))
            throw new FileNotFoundException($"file '{Path}' not found", Path);

        using var stream = File.OpenRead(Path);
        Deserialize(stream);
    }

    public void Save(string Path)
    {
        using var stream = File.Create(Path);
        Serialize(stream);
    }
}

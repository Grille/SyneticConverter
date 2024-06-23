using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files.Common;

namespace SyneticLib.IO;
public abstract class FileSerializer<TFile, TObj> where TFile : BaseFile, new()
{
    public void Save(string filePath, TObj value)
    {
        var file = new TFile();
        Serialize(file, value);
        file.Save(filePath);
    }

    public void Serialize(Stream stream, TObj value)
    {
        var file = new TFile();
        OnSerialize(file, value);
        file.Serialize(stream);
    }

    public void Serialize(TFile file, TObj value)
    {
        OnSerialize(file, value);
    }

    protected abstract void OnSerialize(TFile file, TObj value);

    public TObj Load(string filePath)
    {
        var file = new TFile();
        file.Load(filePath);
        return Deserialize(file);
    }

    public TObj Deserialize(Stream stream)
    {
        var file = new TFile();
        file.Deserialize(stream);
        return OnDeserialize(file);
    }

    public TObj Deserialize(TFile file)
    {
        return OnDeserialize(file);
    }

    public abstract TObj OnDeserialize(TFile file);
}

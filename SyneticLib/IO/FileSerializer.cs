using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyneticLib.Files.Common;

namespace SyneticLib.IO;
public abstract class FileSerializer<TFile, TObj> : IFileSerializer<TObj> where TFile : BaseFile, new()
{
    private List<string> _logs = new List<string>();

    public void Save(string filePath, TObj value)
    {
        var file = new TFile();
        Serialize(file, value);
        file.Save(filePath);
    }

    public void Serialize(Stream stream, TObj value)
    {
        var file = new TFile();
        Serialize(file, value);
    }

    public void Serialize(TFile file, TObj value)
    {
        _logs.Clear();
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
        return Deserialize(stream);
    }

    public TObj Deserialize(TFile file)
    {
        _logs.Clear();
        return OnDeserialize(file);
    }

    protected abstract TObj OnDeserialize(TFile file);

    protected void LogError(string message)
    {
        _logs.Add(message);
    }

    public IReadOnlyCollection<string> GetErrors()
    {
        return _logs;
    }
}

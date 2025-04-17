using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.IO.Generic;
public abstract class DirectoryFileSerializer<TObj> : ISerializer<TObj>
{
    public IReadOnlyCollection<string> GetErrors()
    {
        throw new NotImplementedException();
    }

    private (string DirPath, string FileName) SplitPath(string filePath)
    {
        var fullPath = Path.GetFullPath(filePath);
        var fileName = Path.GetFileNameWithoutExtension(fullPath);
        var dirPath = Path.GetDirectoryName(fullPath);
        if (dirPath == null)
        {
            throw new ArgumentException();
        }
        return (dirPath, fileName);
    }

    public TObj Load(string filePath)
    {
        var (dirPath, fileName) = SplitPath(filePath);
        return OnLoad(dirPath, fileName);
    }

    public TObj Load(string dirPath, string fileName)
    {
        return OnLoad(dirPath, fileName);
    }

    public void Save(string filePath, TObj obj)
    {
        var (dirPath, fileName) = SplitPath(filePath);
        OnSave(dirPath, fileName, obj);
    }

    public void Save(string dirPath, string fileName, TObj obj)
    {
        OnSave(dirPath, fileName, obj);
    }

    protected abstract TObj OnLoad(string dirPath, string fileName);

    protected abstract void OnSave(string dirPath, string fileName, TObj obj);
}

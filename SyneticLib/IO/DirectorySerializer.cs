using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.IO;
public abstract class DirectorySerializer<TObj> : ISerializer<TObj>
{
    public IReadOnlyCollection<string> GetErrors()
    {
        throw new NotImplementedException();
    }

    public TObj Load(string dirPath)
    {
        return OnLoad(dirPath);
    }

    public void Save(string dirPath, TObj obj)
    {
        OnSave(dirPath, obj);
    }

    protected abstract TObj OnLoad(string dirPath);

    protected abstract void OnSave(string dirPath, TObj obj);
}

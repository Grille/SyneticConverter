using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;
public abstract class RessourceFile : Ressource
{
    public override DataState DataState { get; } = DataState.None;

    public bool FileExists => File.Exists(SrcPath);




    public string Ext => Path.GetExtension(SrcPath);


    public void Load() => Load(SrcPath);
    public void Load(string srcPath) => OnLoad(srcPath);
    protected abstract void OnLoad(string srcPath);

    public void Save() => Save(SrcPath);
    public void Save(string dstPath)
    {
        if (DataState == DataState.Changed)
            OnSave(dstPath);

        else if (PointerState == PointerState.Exists)
            File.Copy(SrcPath, dstPath, true);

        else if (DataState == DataState.Loaded)
            OnSave(dstPath);

        throw new Exception();
    }

    protected abstract void OnSave(string dstPath);


    public override void CopyTo(string path) => Save(path);

    public override void LoadAll()
    {
        throw new NotImplementedException();
    }

    public override void SeekAll()
    {
        throw new NotImplementedException();
    }
}

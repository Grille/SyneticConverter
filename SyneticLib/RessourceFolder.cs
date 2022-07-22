using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace SyneticLib;

public abstract class Ressource
{
    string _path;
    public string SrcPath
    {
        get => _path; set
        {
            if (_path == value)
                return;

            _path = value;

            if (_path == null)
                PointerState = PointerState.None;
            else if (FolderExists)
                PointerState = PointerState.Exists;
            else
                PointerState = PointerState.Invalid;
        }
    }

    public string Name => Path.GetFileNameWithoutExtension(SrcPath);

    public PointerState PointerState { get; protected set; } = PointerState.None;

    public abstract DataState DataState { get; }

    public bool FolderExists => Directory.Exists(_path);

    public abstract void SeekAll();

    public abstract void LoadAll();


    public abstract void CopyTo(string path);

}

public class RessourceFolder<T> : Ressource, IList<T> where T : RessourceFile
{
    private List<T> files = new();

    public override DataState DataState
    {
        get
        {
            return DataState.Error;
        }
    }

    public int Count => files.Count;

    public bool IsReadOnly => false;

    public T this[int index] { 
        get => files[index]; 
        set => files[index] = value; 
    }
    public T this[string key] { 
        get => files.Find((x) => x.Name == key);
        set {
            int idx = files.FindIndex((x) => x.Name == key);
            if (idx == -1)
                files.Add(value);
            else
                files[idx] = value;
        }
    }

    public override void LoadAll()
    {
        foreach (var obj in this)
        {
            obj.Load();
        }
    }

    public override void CopyTo(string path)
    {
        foreach (var obj in this)
        {
            string filename = $"{obj.Name}.{obj.Ext}";
            string dstpath = Path.Join(path, filename);
            obj.Save(dstpath);
        }
    }

    public IEnumerator<T> GetEnumerator() => files.GetEnumerator();
    
    IEnumerator IEnumerable.GetEnumerator() => files.GetEnumerator();
    
    public int IndexOf(T item) => files.IndexOf(item);

    public void Insert(int index, T item) => files.Insert(index, item);

    public void RemoveAt(int index) => files.RemoveAt(index);

    public void Add(T item) => files.Add(item);

    public void Clear() => files.Clear();

    public bool Contains(T item) => files.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => files.CopyTo(array, arrayIndex);

    public bool Remove(T item) => files.Remove(item);

    public override void SeekAll()
    {
        throw new NotImplementedException();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;
public class RessourceList<T> : Ressource, IList<T> where T : Ressource
{
    public List<T> Items;

    public RessourceList(Ressource parent, PointerType type = PointerType.Virtual) : base(parent, type)
    {
        Items = new();
    }


    public T this[int index] { get => Items[index]; set => Items[index] = value; }

    public int Count => Items.Count;

    public bool IsReadOnly => false;

    public void Add(T item) => Items.Add(item);

    public void Clear() => Items.Clear();

    public bool Contains(T item) => Items.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

    public int IndexOf(T item)
    {
        throw new NotImplementedException();
    }

    public void Insert(int index, T item)
    {
        throw new NotImplementedException();
    }

    public bool Remove(T item)
    {
        throw new NotImplementedException();
    }

    public void RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    protected override void OnLoad()
    {
        throw new NotImplementedException();
    }

    protected override void OnSave()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }



    protected override void OnSeek()
    {
        throw new NotImplementedException();
    }
}

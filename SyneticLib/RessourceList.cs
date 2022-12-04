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
    public readonly List<T> Items;

    public RessourceList(Ressource parent, string path, PointerType type = PointerType.Virtual) : base(parent, path, type)
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
    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

    public int IndexOf(T item) => Items.IndexOf(item);

    public void Insert(int index, T item) => Items.Insert(index, item);

    public bool Remove(T item) => Items.Remove(item);

    public void RemoveAt(int index) => Items.RemoveAt(index);

    protected override void OnLoad()
    {
        foreach (var item in Items)
        {
            item.Load();
        }
    }
}

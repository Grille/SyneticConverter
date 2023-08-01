using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyneticLib;
public class RessourceList<T> : IReadOnlyList<T> where T : Ressource
{
    public readonly List<T> Items;

    public RessourceList()
    {
        Items = new();
    }

    public T this[int index] { 
        get => Items[index]; 
    }

    public int Count => Items.Count;

    public bool IsReadOnly => true;

    public bool Contains(T item) => Items.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

    public int IndexOf(T item) => Items.IndexOf(item);
}

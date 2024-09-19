using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyneticLib.Collections;
internal class SyneticObjectCollection<T> : ICollection<T> where T : SyneticObject
{
    readonly List<T> _list;

    public SyneticObjectCollection()
    {
        _list = new List<T>();
    }

    public int Count => _list.Count;

    public bool IsReadOnly => false;

    public void Add(T item)
    {
        _list.Add(item);
    }

    public void Clear()
    {
        _list.Clear();
    }

    public bool Contains(T item)
    {
        return _list.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _list.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    public bool Remove(T item)
    {
        return _list.Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    public T[] CreateIndexedArray(IList<string> names)
    {
        var result = new T[names.Count];
        for (var i = 0; i < names.Count; i++)
        {
            result[i] = GetByName(names[i].ToString());
        }
        return result;
    }

    public T GetByName(string key)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].Equals(key)) return _list[i];
        }
        throw new KeyNotFoundException();
    }
}

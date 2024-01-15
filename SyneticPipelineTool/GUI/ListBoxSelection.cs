using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticPipelineTool.GUI;

internal class ListBoxSelection<T> : IReadOnlyList<T> where T : class
{
    public ListBoxSelection(ListBox.SelectedObjectCollection collection)
    {
        var list = new List<T>();
        foreach (var item in collection)
        {
            list.Add((T)item);
        }
    }

    List<T> list;

    public T this[int index] => ((IReadOnlyList<T>)list)[index];

    public int Count => ((IReadOnlyCollection<T>)list).Count;

    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)list).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)list).GetEnumerator();
    }
}

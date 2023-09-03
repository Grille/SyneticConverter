using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticPipelineTool.GUI;

public abstract class ListBox<T> : ListBox where T : class
{
    public new T SelectedItem
    {
        get => (T)base.SelectedItem;
        set => base.SelectedItem = value;
    }

    AsyncPipelineExecuter _executer;
    public AsyncPipelineExecuter Executer { 
        get => _executer; 
        set
        {
            if (value == null)
                return;
            
            _executer = value;
        }
    }

    public ListBox()
    {
        DrawMode = DrawMode.OwnerDrawFixed;
        DoubleBuffered = true;
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        if (Executer == null)
            return;

        if (e.Index == -1)
            return;

        if (e.State.HasFlag(DrawItemState.Selected))
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.LightBlue), e.Bounds);
        }
        else
        {
            e.Graphics.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);
        }

        var item = (T)Items[e.Index];

        try
        {
            OnDrawItem(e, item);
        }
        catch { }
    }

    protected abstract void OnDrawItem(DrawItemEventArgs e, T item);

    public void UpdateItems(List<T> list)
    {
        var items = Items;

        if (list == null)
        {
            items.Clear();
            return;
        }

        BeginUpdate();

        var selectet = SelectedItem;

        if (items.Count == list.Count)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (items[i] != list[i])
                {
                    items[i] = list[i];
                }
            }
        }
        else
        {
            items.Clear();
            foreach (var item in list)
            {
                items.Add(item);
            }
            SelectedItem = selectet;
        }

        if (selectet != null && SelectedItem != selectet)
            SelectedItem = selectet;

        Invalidate();
        EndUpdate();
    }
}

using SyneticPipelineTool.Tasks;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticPipelineTool.GUI;

public class PipelineTaskListBox : ListBox<PipelineTask>
{
    public List<PipelineTask> SelectedTasks
    {
        get
        {
            var list = new List<PipelineTask>();
            foreach (var item in SelectedItems)
            {
                list.Add((PipelineTask)item);
            }
            return list;
        }
        set
        {
            ClearSelected();
            foreach (var task in value)
            {
                int idx = Items.IndexOf(task);
                SetSelected(idx, true);
            }
        }
    }

    protected override void OnDrawItem(DrawItemEventArgs e, PipelineTask task)
    {
        var pipeline = task.Pipeline;

        var g = e.Graphics;

        Brush brushLineBack = Brushes.Gainsboro;
        Brush brushLine = Brushes.DimGray;
        //Brush brushText;

        /*
        if (task is NopTask)
            brushText = new SolidBrush(Color.Gray);
        else if (task is InvalidTypeTask)
            brushText = new SolidBrush(Color.Red);
        else
            brushText = new SolidBrush(Color.Black);
        */

        var entry = Executer.Runtime.CallStack.ToList().FirstOrDefault(a => a.Pipeline == pipeline, null);
        if (entry != null && entry.Position == e.Index)
        {
            brushLineBack = Brushes.LightGreen;
            brushLine = Brushes.DarkGreen;
        }


        int lineColumnWidth = 24;

        var boundsLine = (RectangleF)e.Bounds;
        boundsLine.Width = lineColumnWidth;
        var boundsText = (RectangleF)e.Bounds;
        boundsText.Width -= lineColumnWidth;
        boundsText.X += lineColumnWidth;

        g.FillRectangle(brushLineBack, boundsLine);

        g.DrawString((e.Index + 1).ToString(), e.Font, brushLine, boundsLine);

        float margin = g.MeasureString(" ", e.Font).Width;
        float charsize = g.MeasureString("O",e.Font).Width - margin;

        var tokens = task.ToTokens();
        float position = boundsText.X + (charsize * task.Scope * 4);



        var format = new StringFormat(StringFormatFlags.MeasureTrailingSpaces);

        foreach (var token in tokens)
        {
            var text = token.Text;

            if (string.IsNullOrEmpty(text))
                continue;

            var size = g.MeasureString(text, e.Font, 0, format);
            //var size = TextRenderer.MeasureText(text, e.Font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);

            var drawrect = new RectangleF(position, boundsText.Y, size.Width, size.Height);

            var color = token.Type switch
            {
                PipelineTask.TokenType.Text => Color.Black,
                PipelineTask.TokenType.Comment => Color.Gray,
                PipelineTask.TokenType.Variable => text[0] == '*' || text[0] == '$' ? Color.Blue : Color.FromArgb(0,100,100),
                PipelineTask.TokenType.Flow => Color.DarkMagenta,
                _ => Color.Red,
            };

            var brush = new SolidBrush(color);

            //g.DrawRectangle(Pens.Red, drawrect.X,drawrect.Y,drawrect.Width-1,drawrect.Height-1);
            g.DrawString(token.Text, e.Font, brush, drawrect);

            position += size.Width - margin;
        }

        //g.DrawString(task.ToString(), e.Font, Brushes.Magenta, boundsText);
    }


}

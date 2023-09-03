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

public class PipelineListBox : ListBox<Pipeline>
{
    protected override void OnDrawItem(DrawItemEventArgs e, Pipeline pipeline)
    {
        var g = e.Graphics;

        Brush brushLineBack = Brushes.Gainsboro;
        Brush brushLine = Brushes.DimGray;
        Brush brushText;

        brushText = new SolidBrush(Color.Black);

        bool stackContains = Executer.Runtime.CallStack.ToList().Any(a => a.Pipeline == pipeline);

        if (stackContains)
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


        if (stackContains)
        {
            var list = Executer.Runtime.CallStack.ToList();
            list.Reverse();
            int stackIdx = list.FindIndex(a => a.Pipeline == pipeline);
            g.DrawString((stackIdx + 1).ToString(), e.Font, brushLine, boundsLine);
        }


        g.DrawString(pipeline.ToString(), e.Font, brushText, boundsText);

        //base.OnDrawItem(e);
    }
}

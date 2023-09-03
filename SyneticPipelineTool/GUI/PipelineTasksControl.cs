using SyneticPipelineTool.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyneticPipelineTool.GUI
{
    public partial class PipelineTasksControl : UserControl
    {
        public Pipeline Pipeline { get; set; }

        public PipelineTask SelectedItem
        {
            get => ListBox.SelectedItem;
            set => ListBox.SelectedItem = value;
        }

        public List<PipelineTask> SelectedItems
        {
            get => ListBox.SelectedTasks;
            set => ListBox.SelectedTasks = value;
        }


        public PipelineTasksControl()
        {
            InitializeComponent();
        }


        public void SetEnabledActions()
        {
            bool single = SelectedItems.Count == 1;
            bool multi = SelectedItems.Count > 1;
            bool invalid = SelectedItem is InvalidTypeTask;


            copyToolStripMenuItem.Enabled = ButtonCopy.Enabled = single && !invalid;
            editToolStripMenuItem.Enabled = ButtonEdit.Enabled = single;
            removeToolStripMenuItem.Enabled = ButtonRemove.Enabled = (single || multi);

            upToolStripMenuItem.Enabled = ButtonUp.Enabled = (single || multi);
            downToolStripMenuItem.Enabled = ButtonDown.Enabled = (single || multi);
        }

        public void InvalidateItems(bool save = true)
        {
            if (Pipeline == null)
            {
                ListBox.UpdateItems(null);
            }
            else
            {
                ListBox.UpdateItems(Pipeline.Tasks);
            }
            SetEnabledActions();

            //if (save)
            //    Pipeline.Owner.Save();
        }


        private void NewClick(object sender, EventArgs e)
        {
            var dialog = new EditTaksForm(Pipeline);
            var result = dialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                Pipeline.Tasks.InsertAfter(SelectedItem, dialog.Task);
                InvalidateItems();
                SelectedItem = dialog.Task;
                SelectedItem.Pipeline = Pipeline;
            }
        }


        private void EditClick(object sender, EventArgs e)
        {
            var dialog = new EditTaksForm(Pipeline, SelectedItem);
            var result = dialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                dialog.Task.Pipeline = Pipeline;
                int idx = Pipeline.Tasks.IndexOf(SelectedItem);
                Pipeline.Tasks[idx] = dialog.Task;
                InvalidateItems();
            }
        }

        private void DeleteClick(object sender, EventArgs e)
        {
            foreach (var task in SelectedItems)
            {
                Pipeline.Tasks.Remove(task);
            }
            InvalidateItems();
        }

        private void CopyClick(object sender, EventArgs e)
        {
            var clone = SelectedItem.Clone();
            clone.Pipeline = Pipeline;
            Pipeline.Tasks.InsertAfter(SelectedItem, clone);
            InvalidateItems();
        }

        private void UpClick(object sender, EventArgs e)
        {
            var selected = SelectedItems;
            foreach (var task in selected)
            {
                Pipeline.Tasks.UpItem(task);
            }
            InvalidateItems();
            SelectedItems = selected;
        }

        private void DownClick(object sender, EventArgs e)
        {
            var selected = SelectedItems;
            selected.Reverse();
            foreach (var task in selected)
            {
                Pipeline.Tasks.DownItem(task);
            }
            InvalidateItems();
            SelectedItems = selected;
        }

        private void ListBoxDoubleClick(object sender, EventArgs e)
        {
            if (SelectedItem == null || SelectedItem is InvalidTypeTask)
                return;
            EditClick(sender, e);
        }

        private void ListBoxIndexChanged(object sender, EventArgs e)
        {
            SetEnabledActions();
        }
    }
}
